using Amazon.S3.Transfer;
using Amazon.S3;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using TiffinMate.DAL.Entities.AWS;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using static Supabase.Gotrue.Constants;
using System.Security.Claims;
using Supabase.Gotrue;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;

namespace TiffinMate.BLL.Services.ProviderServices
{
    public class ProviderService : IProviderService
    {
        private readonly IAmazonS3 _s3Client;
        //private readonly string _bucketName;
        private readonly IProviderRepository _providerRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;


        public ProviderService(IAmazonS3 s3Client, IProviderRepository providerRepository, IConfiguration configuration, IMapper mapper)
        {
            _s3Client = s3Client;
            //string _bucketName = AwsConfig.BucketName;
            _providerRepository = providerRepository;
            _configuration = configuration;
            _mapper = mapper;

        }

        public async Task<string> UploadCertificateToS3(Stream certificateStream, string uniqueFileName, string contentType)
        {
            try
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = certificateStream,
                    Key = uniqueFileName,
                    BucketName = AwsConfig.BucketName,
                    ContentType = contentType
                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);

                // Generate the file URL after uploading to S3
                var region = _s3Client.Config.RegionEndpoint.SystemName;
                var certificateUrl = $"https://{AwsConfig.BucketName}.s3.{region}.amazonaws.com/{uniqueFileName}";
               


                return certificateUrl;
                
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while uploading the certificate to S3.", ex);
            }
        }


        public async Task<LoginResponse> LoginProvider(ProviderLoginDTO loginData)
        {
            var provider = await _providerRepository.Login(loginData.email, loginData.password);
            if (provider == null)
            {
                return new LoginResponse
                {
                    Token = null,
                    Message = "Invalid email or password"
                };
            }

            var token = GenerateJwtToken(  provider);
            return new LoginResponse
            {
                Id = provider.id.ToString(),
                username = provider.username,
                Token = token,
                Message = "Login successful."
            };
        }
        private string GenerateJwtToken(DAL.Entities.ProviderEntity.Provider provider)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, provider.id.ToString()),
                new Claim(ClaimTypes.Name, provider.username),

            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddDays(1)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> GenereateAndSendPassword(Guid ProviderId)
        {
            var provider = await _providerRepository.GetProviderById(ProviderId);
            if (provider == null)
            {
                return "Provider not found";
            }
            var newPassword = GenerateRandomPassword();
            var passwordHasher = new PasswordHasher<DAL.Entities.ProviderEntity.Provider>();
            var hashPasssword = passwordHasher.HashPassword(provider, newPassword);
            provider.password = hashPasssword;
            _providerRepository.Update(provider);
            _providerRepository.SaveChangesAsync();
            var emailSend = await SendPasswordEmailAsync(provider.email, provider.password);

            if (emailSend)
            {
                return "Password has been updated and sent to the provider's email.";
            }
            else
            {
                return "Password updated, but failed to send email.";
            }

        }
        private string GenerateRandomPassword()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[5]; 
                rng.GetBytes(byteArray);

                // Convert byte array to a Base64 string to create a password
                var password = Convert.ToBase64String(byteArray);
                return password;
            }
        }
        private async Task<bool> SendPasswordEmailAsync(string email, string password)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("sherinshamna78@gmail.com"), 
                    Subject = "Your New Account Password",
                    Body = $"Your new password is: {password}",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                // configure
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("sherinshamna78@gmail.com", "your-app-password"),
                    EnableSsl = true
                };
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
             
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }

}
