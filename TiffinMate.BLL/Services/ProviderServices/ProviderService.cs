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

namespace TiffinMate.BLL.Services.ProviderServices
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinary;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public ProviderService(IProviderRepository providerRepository, IMapper mapper, ICloudinaryService cloudinary, IConfiguration config, AppDbContext context)
        {
            _providerRepository = providerRepository;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _config = config;
           _context=context;
        }


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
        public async Task<ProviderLoginResponse> AddLogin(ProviderLoginDTO providerdto)
        {
            try
            {
                var pro = await _providerRepository.Login(providerdto.email, providerdto.password);
                if (pro == null)
                {
                  
                    throw new Exception("Invalid provider.");
                }
                if (pro.password != providerdto.password)
                {
                    throw new Exception("Incorrect password.");
                }

                var token = CreateToken(pro);

                return new ProviderLoginResponse
                {
                    id = pro.id,
                    email = pro.email,
                    token = token
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred: " + ex.Message);
            }
        }
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

                await _providerRepository.AddProviderDetailsAsync(prddetails);
                await _providerRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private string CreateToken(Provider user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
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

        public async Task<List<Provider>> GetProviders()
        {
            var provider = await _providerRepository.GetProviders();
            return provider;
        }


    }
}
