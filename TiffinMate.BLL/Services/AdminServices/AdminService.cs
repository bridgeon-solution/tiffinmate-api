using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.AdmiDTO;
using TiffinMate.BLL.DTOs.AdmiDTOs;
using TiffinMate.BLL.Interfaces.AdminInterface;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.AdminInterfaces;

namespace TiffinMate.BLL.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _config;
        private readonly string _jwtKey;
        public AdminService(IAdminRepository adminRepository,IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _config = configuration;
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        }
        public async Task<LoginResponseDTO> AdminLogin(AdminLoginDTO adminLoginDTO)
        {
            
            var user = await _adminRepository.AdminLogin(adminLoginDTO.email);
            if (user == null)
            {
                return new LoginResponseDTO
                {
                    message = "Not Found"
                };
            }
            if (user.password != adminLoginDTO.password)
            {
                return new LoginResponseDTO
                {
                    message = "inccorect password"

                };
            }
            var tokenHelper = new TokenHelper();

            var newRefreshToken = tokenHelper.GenerateRefreshTokenAdmin(user);

            if (string.IsNullOrEmpty(newRefreshToken))
            {
                throw new Exception("Failed to generate refresh token.");
            }

            user.refresh_token = newRefreshToken;
            user.refreshtoken_expiryDate = DateTime.UtcNow.AddDays(7);
            user.updated_at = DateTime.UtcNow;
            var token = CreateToken(user);
            return new LoginResponseDTO
            {
                id = user.id,
                name = user.user_name,
                token = token,
                message="success",
                refresh_token= newRefreshToken
            };


            
        }

        private string CreateToken(Admin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim (ClaimTypes.Name,user.user_name),
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
    }
}
