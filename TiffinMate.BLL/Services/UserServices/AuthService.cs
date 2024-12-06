using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.AuthInterface;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;

namespace TiffinMate.BLL.Services.UserService
{
    public class AuthService:IAuthService
    {
        private readonly IAuthRepository _userRepository;
        private readonly IOtpService _otpService;
        private readonly IConfiguration _configuration;
        private static Dictionary<string,RegisterUserDto> _otpStore= new Dictionary<string,RegisterUserDto>();
        public AuthService(IAuthRepository userRepository,IOtpService otpService,IConfiguration configuration)
        {
            _userRepository = userRepository;
            _otpService = otpService;
            _configuration = configuration;
            
        }
        public async Task<bool> RegisterUser(RegisterUserDto userDto)
        {
            if (await _userRepository.UserExists(userDto.email))
            {
                return false;

            }
            
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(userDto.password);
            userDto.password = hashPassword;
            
            _otpStore[userDto.phone] = userDto;

            var otpSent = await _otpService.SendSmsAsync(userDto.phone);
            return true;
        }
        public async Task<bool> VerifyUserOtp(string phone, string otp)
        {
            var isValid=await _otpService.VerifyOtpAsync(phone, otp);
            if (isValid)
            {
                if (_otpStore.TryGetValue(phone, out var userDto))
                {
                    var user = new User
                    {
                        name = userDto.name,
                        email = userDto.email,
                        phone = phone,
                        created_at = DateTime.UtcNow,
                        is_blocked = false,
                        password = userDto.password,
                        updated_at = DateTime.UtcNow,
                    };
                    await _userRepository.CreateUser(user);
                    _otpStore.Remove(phone);
                    return true;

                }
                return false;
               


            }
            return false;
        }
        public async Task<string>LoginUser(LoginUserDto userDto)
        {
            var user = await _userRepository.GetUserByEmail(userDto.email);
            if (user == null)
            {
                return "Not Found";

            }
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDto.password, user.password);
            if (!isPasswordValid)
            {
                return "Invalid Password";
            }
            var token = GenerateJwtToken(user);

            return token;


        }
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim (ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim (ClaimTypes.Name,user.name),
                new Claim (ClaimTypes.Email, user.email),
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
