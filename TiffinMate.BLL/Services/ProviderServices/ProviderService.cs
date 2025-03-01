using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using TiffinMate.DAL.Entities.ProviderEntity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using TiffinMate.BLL.Interfaces.CloudinaryInterface;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using CloudinaryDotNet.Actions;
using sib_api_v3_sdk.Client;
using System.Net;
using TiffinMate.BLL.DTOs.UserDTOs;
using Supabase.Gotrue;
using BCrypt.Net;
using TiffinMate.BLL.Interfaces.NotificationInterface;
using static Supabase.Gotrue.Constants;
using Org.BouncyCastle.Cms;
using Twilio.TwiML.Messaging;

using static Supabase.Gotrue.Constants;
using TiffinMate.BLL.Interfaces.NotificationInterface;
using Provider = TiffinMate.DAL.Entities.ProviderEntity.Provider;
using TiffinMate.BLL.DTOs;
using System.Collections.Generic;
namespace TiffinMate.BLL.Services.ProviderServices
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinary;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly string _jwtKey;
        private readonly INotificationService _notificationService;
        public ProviderService(IProviderRepository providerRepository, IMapper mapper, ICloudinaryService cloudinary, IConfiguration config, AppDbContext context,INotificationService notificationService)
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _config = config;
            _context = context;
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            _notificationService = notificationService;
           
        }

        //register
        public async Task<bool> AddProvider(ProviderDTO provider, IFormFile certificateFile)
        {
            try
            {
                if (certificateFile == null || certificateFile.Length == 0)
                {
                    return false;
                }

                var providerss = await _providerRepository.EmailExistOrNot(provider.email);
                if (providerss)
                {
                    throw new Exception("Email already exist" );
                }
                var certificateUrl = await _cloudinary.UploadDocumentAsync(certificateFile);

                var prd = _mapper.Map<TiffinMate.DAL.Entities.ProviderEntity.Provider>(provider);

                prd.certificate = certificateUrl;
                await _providerRepository.AddProviderAsync(prd);
                await _providerRepository.SaveChangesAsync();
                var adminTitle = "Provider Registration";
                var adminMessage = $"New provider registered: {provider.user_name}.";
                await _notificationService.NotifyAdminsAsync(
                  adminTitle, adminMessage, "Registration"
                    
                );
                return true;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        //addlogin
        public async Task<ProviderLoginResponse> AddLogin(ProviderLoginDTO providerdto)
        {
            try
            {
                if (string.IsNullOrEmpty(providerdto.email) || string.IsNullOrEmpty(providerdto.password))
                {
                    throw new Exception("Email or password cannot be empty");
                }

                var pro = await _providerRepository.Login(providerdto.email);

                if (pro == null)
                {
                    throw new Exception("Invalid email");
                }
                if (pro.is_blocked)
                {
                    throw new Exception("Your account has been blocked. Please contact support");
                }
                switch (pro.verification_status?.ToLower())
                {
                    case "rejected":
                        throw new Exception("Your account has been rejected. Please contact support");
                    case "pending":
                        throw new Exception("Your account is pending approval. Please wait for admin verification");
                }
                bool isValid = BCrypt.Net.BCrypt.Verify(providerdto.password, pro.password);

                if (!isValid)
                {
                    throw new Exception("Invalid password");
                }
                var tokenHelper = new TokenHelper();

                var newRefreshToken = tokenHelper.GenerateRefreshToken(pro);

                if (string.IsNullOrEmpty(newRefreshToken))
                {
                    throw new Exception("Failed to generate refresh token.");
                }

                pro.refresh_token = newRefreshToken;
                pro.refreshtoken_expiryDate = DateTime.UtcNow.AddDays(7);
                pro.updated_at = DateTime.UtcNow;
                var tokens = new ProviderToken();
                var token = tokens.CreateTokenProvider(pro);
                //var token = CreateToken(pro);
                _providerRepository.Update(pro);
                await _providerRepository.SaveChangesAsync();

                return new ProviderLoginResponse
                {
                    id = pro.id,
                    email = pro.email,
                    token = token,
                    refresh_token = newRefreshToken
                };

            }
            catch(Exception ex)
            {
                
                throw new Exception(ex.Message);              

            }
        }

        //addproviderdetails
        public async Task<bool> AddProviderDetails(ProviderDetailsDTO providerDetailsdto, IFormFile logo, IFormFile image)
        {
            try
            {
                if (logo == null || image == null)
                {
                    return false;
                }

                // Check if provider details already exist
                var existingDetails = await _providerRepository.GetProviderDetailsByProviderIdAsync(providerDetailsdto.provider_id);
                if (existingDetails != null)
                {
                    throw new Exception("Provider details already exist.");
                }

                var logUrl = await _cloudinary.UploadDocumentAsync(logo);
                var imageUrl = await _cloudinary.UploadDocumentAsync(image);

                var prddetails = _mapper.Map<ProviderDetails>(providerDetailsdto);

                prddetails.logo = logUrl;
                prddetails.image = imageUrl;
                prddetails.updated_at = DateTime.UtcNow;

                await _providerRepository.AddProviderDetailsAsync(prddetails);
                await _providerRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //get refresh token
        public async Task<ProviderLoginResponse> GetRefreshToken(string refreshToken)
        {
            try
            {
                var provider = await _providerRepository.GetUserByRefreshTokenAsync(refreshToken);
                if (provider == null || provider.refreshtoken_expiryDate < DateTime.UtcNow)
                {
                    throw new Exception("Invalid or expired refresh token.");
                }
                else
                {
                    var tokens = new ProviderToken();
                    var newAccessToken = tokens.CreateTokenProvider(provider);
                    var tokenHelper = new TokenHelper();
                    var newRefreshToken = tokenHelper.GenerateRefreshToken(provider);

                    //update
                    provider.refresh_token = newRefreshToken;
                    provider.refreshtoken_expiryDate = DateTime.UtcNow.AddDays(7);
                    provider.updated_at = DateTime.UtcNow;

                    return new ProviderLoginResponse
                    {
                        id = provider.id,
                        email = provider.email,
                        token = newAccessToken,
                        refresh_token = newRefreshToken,
                    };
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
       
        //get all provider


        public async Task<BlockUnblockResponse> BlockUnblock(Guid id)
        {
            var user = await _providerRepository.BlockUnblockUser(id);
            if (user != null)
            {
                user.is_blocked = !user.is_blocked;
                _context.SaveChanges();
                return new BlockUnblockResponse
                {
                    is_blocked = user.is_blocked == true ? true : false,
                    message = user.is_blocked == true ? "user is blocked" : "user is unblocked"
                };
            }

            return new BlockUnblockResponse
            {
                message = "invalid user"
            };
        }

        public async Task<ProviderResultDTO> GetProviders(int page, int pageSize, string search = null, string filter = null, string verifystatus = null)
        {
            var providerList = await _providerRepository.GetProviders();
            if (providerList == null)
            {
                throw new InvalidOperationException("No providers found.");
            }
            var providers = providerList.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                providers = providers.Where(u =>
                    (u.user_name != null && u.user_name.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                    (u.email != null && u.email.Contains(search, StringComparison.OrdinalIgnoreCase)));
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.ToLower() == "true")
                {
                    providers = providers.Where(u => u.is_blocked);
                }
                else if (filter.ToLower() == "false")
                {
                    providers = providers.Where(u => !u.is_blocked);
                }
            }

            if (!string.IsNullOrEmpty(verifystatus))
            {
                providers = providers.Where(u =>
                    u.verification_status != null &&
                    u.verification_status.Equals(verifystatus, StringComparison.OrdinalIgnoreCase));
            }

            var totalCount = providers.Count();

            var pagedProviders = providers
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ProviderResultDTO
            {
                TotalCount = totalCount,
                Providers = _mapper.Map<List<ProviderResponseDTO>>(pagedProviders)
            };
        }
        public async Task<ProviderByIdDto> ProviderById(Guid providerId)
        {
            var provider = await _providerRepository.GetAProviderById(providerId);
            if (provider == null)
            {
                throw new Exception("No provider available");
            }
            var result = provider.Select(providers => new ProviderByIdDto
            {
                username = providers.user_name,
                email = providers.email,
                address = providers.provider_details.address, 
                phone_no = providers.provider_details.phone_no,
                verification_status = providers.verification_status,
                image = providers.provider_details.logo,
                created_at = providers.created_at,
                certificate = providers.certificate,

            }).FirstOrDefault();
            return result;
        }
        public async Task<ProviderDetailedDTO> GetProviderDetailsById(Guid id)
        {
            try
            {
                var res = await _providerRepository.GetProviderDetailsById(id);
                var provider= _mapper.Map<ProviderDetailedDTO>(res);
                provider.certificate = res.Provider.certificate;
                provider.email = res.Provider.email;
                provider.user_name = res.Provider.user_name;

                return provider;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProviderDetailResponse>> GetProvidersWithDetail()
        {
            try
            {
                var res = await _providerRepository.GetProvidersWithDetail();
                return _mapper.Map<List<ProviderDetailResponse>>(res);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> EditDetails(EditDetailsDto providerDetailsdto, IFormFile logo)
        {
            try
            {

                var provider = await _providerRepository.GetProviderById(providerDetailsdto.provider_id);
                if (provider == null)
                {
                    throw new Exception("No provider available with the specified ID.");
                }


                var existingDetails = await _providerRepository.GetProviderDetailsByProviderIdAsync(providerDetailsdto.provider_id);
                if (existingDetails == null)
                {
                    throw new Exception("Provider details not found.");
                }

                if (logo != null && logo.Length > 0)
                {
                    var logoUrl = await _cloudinary.UploadDocumentAsync(logo);
                    existingDetails.logo = logoUrl;
                }



                existingDetails.Provider.email = providerDetailsdto.email;
                existingDetails.Provider.user_name = providerDetailsdto.username;
                existingDetails.address = providerDetailsdto.address;
                existingDetails.phone_no = providerDetailsdto.phone_no;
                existingDetails.updated_at = DateTime.UtcNow;

                _providerRepository.UpdateDetails(existingDetails);
                await _providerRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while editing the provider details: " + ex.Message);
            }
        }
        //details exist or not 
        public async Task<bool> DetailsExist(Guid id)
        {
            try
            {
                var isExist = await _providerRepository.DetailsExistOrNot(id);
                if (isExist)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
        }
        public async Task<List<PaymentHistoryDto>> GetPaymentByProvider(Guid providerId)
        {
            var payment = await _providerRepository.GetPaymentByProvider(providerId);
            var details = payment.Select(p => new PaymentHistoryDto
            {
                amount = p.amount,
                payment_date = p.payment_date,
                is_paid = p.is_paid,
                user_name = p.user.name
            }).ToList();
            return details;
        }
        public async Task<List<AllTransactionByProviderDto>>GetTransactionByProviderId(Guid providerId,int page,int pageSize,string search=null )
        {
            var payment = await _providerRepository.GetPaymentByProvider(providerId);
            if (!string.IsNullOrEmpty(search))
            {
                payment = payment.Where(u => u.user.name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            var paginatedPayments = payment.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                var details = paginatedPayments.Select(p => new PaymentHistoryDto
                {
                    id=p.id,
                    amount = p.amount,
                    payment_date = p.payment_date,
                    is_paid = p.is_paid,
                    user_name = p.user.name
                }).ToList();
                return new  List < AllTransactionByProviderDto >
                {
                   new AllTransactionByProviderDto
                   {
                       TotalCount = payment.Count,
                       PaymentHistory=details,
                   }
                };

            }
        }
    }


