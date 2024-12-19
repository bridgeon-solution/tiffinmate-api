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
        public ProviderService(IProviderRepository providerRepository, IMapper mapper, ICloudinaryService cloudinary, IConfiguration config, AppDbContext context)
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _config = config;
            _context = context;
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        }

        //register
        public async Task<bool> AddProvider(ProviderDTO product, IFormFile certificateFile)
        {
            try
            {
                if (certificateFile == null || certificateFile.Length == 0)
                {
                    return false;
                }


                var certificateUrl = await _cloudinary.UploadDocumentAsync(certificateFile);

                var prd = _mapper.Map<Provider>(product);

                prd.certificate = certificateUrl;
                await _providerRepository.AddProviderAsync(prd);
                await _providerRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.InnerException?.Message ?? ex.Message);
                throw new Exception("An error occurred while adding the product: " + ex.Message);
            }
        }

        //addlogin
        public async Task<ProviderLoginResponse> AddLogin(ProviderLoginDTO providerdto)
        {
            try
            {
                if (string.IsNullOrEmpty(providerdto.email) || string.IsNullOrEmpty(providerdto.password))
                {
                    throw new Exception("Email or password cannot be null or empty.");
                }

                var pro = await _providerRepository.Login(providerdto.email, providerdto.password);

                if (pro == null)
                {
                    throw new Exception("Invalid provider.");
                }

                if (pro.password == null || providerdto.password == null)
                {
                    throw new Exception("Password cannot be null.");
                }

                if (pro.password != providerdto.password)
                {
                    throw new Exception("Incorrect password.");
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

                var token = CreateToken(pro);
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
            catch (Exception ex)
            {

                Console.WriteLine($"Error during login: {ex.Message}");
                Console.WriteLine($"Query Parameters - Email: {providerdto.email}, Password: {providerdto.password}");

                throw new Exception("An error occurred: " + ex.Message);
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

                var logUrl = await _cloudinary.UploadDocumentAsync(logo);
                var imageUrl = await _cloudinary.UploadDocumentAsync(image);

                //var providerDetails = new ProviderDetails
                //{

                //    resturent_name = providerDetailsdto.resturent_name,
                //    address = providerDetailsdto.address,
                //    phone_no = providerDetailsdto.phone_no,
                //    location = providerDetailsdto.location,
                //    logo = logUrl,
                //    image = imageUrl,
                //    about = providerDetailsdto.about,
                //    account_no = providerDetailsdto.account_no
                //};


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
                var newAccessToken = CreateToken(provider);
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
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        //Token
        private string CreateToken(Provider user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim (ClaimTypes.Name,user.username),
                new Claim (ClaimTypes.Role, user.role),
                new Claim(ClaimTypes.Email, user.email)
            };

            var token = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: credentials,
                    expires: DateTime.Now.AddDays(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //get all provider

        public async Task<List<ProviderResponseDTO>> GetProviders()
        {
            var provider = await _providerRepository.GetProviders();
            return _mapper.Map<List<ProviderResponseDTO>>(provider);
        }

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

   public async Task <ProviderByIdDto> ProviderById(Guid providerId)
        {
            var provider = await _providerRepository.GetAProviderById(providerId);
            if (provider == null)
            {
                throw new Exception("No provider available");
            }
            var result = provider.Select(providers => new ProviderByIdDto
            {
                username = providers.username,
                email = providers.email,
                address = providers.ProviderDetails.address,
                phone_no = providers.ProviderDetails.phone_no,
                verification_status = providers.verification_status,
                image = providers.ProviderDetails.logo,
                created_at = providers.created_at,
                certificate = providers.certificate,
               
            }).FirstOrDefault();
            return result;
        }




    }
}

