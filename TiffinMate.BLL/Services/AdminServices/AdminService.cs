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
using TiffinMate.BLL.Interfaces.AdminInterface;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.AdminInterfaces;

namespace TiffinMate.BLL.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _config;
      
        public AdminService(IAdminRepository adminRepository,IConfiguration configuration)
        {
            _adminRepository = adminRepository;
            _config = configuration;
        }
        public async Task<string> AdminLogin(AdminLoginDTO adminLoginDTO)
        {
            
            var user = await _adminRepository.AdminLogin(adminLoginDTO.email);
            if (user == null)
            {
                return "Not Found"; 
            }
            if (user.password != adminLoginDTO.password)
            {
                return "invalid password";
            }
            var token = CreateToken(user);
            return token;


            
        }

        private string CreateToken(Admin user)
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
    }
}
