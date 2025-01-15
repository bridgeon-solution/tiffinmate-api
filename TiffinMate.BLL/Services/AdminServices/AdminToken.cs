using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;

namespace TiffinMate.BLL.Services.AdminServices
{
    public class AdminToken
    {
        private readonly string _jwtKey;
        public AdminToken()
        {
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        }
        public string CreateAdminToken(Admin user)
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
