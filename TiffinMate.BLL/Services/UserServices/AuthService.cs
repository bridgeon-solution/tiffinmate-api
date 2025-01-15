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
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.BLL.Services.AdminServices;
using TiffinMate.BLL.Services.UserServices;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;

namespace TiffinMate.BLL.Services.UserService
{
    public class AuthService:IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IOtpService _otpService;
        private readonly IBrevoMailService _brevoMailService;
        private readonly IConfiguration _configuration;
        private static Dictionary<string,RegisterUserDto> _otpStore= new Dictionary<string,RegisterUserDto>();
        private static Dictionary<string, string> _emailOtpStore = new Dictionary<string, string>();
        private readonly string _jwtKey;
        public AuthService(IAuthRepository authRepository,IOtpService otpService,IConfiguration configuration,IBrevoMailService brevoMailService)
        {
            _authRepository = authRepository;
            _otpService = otpService;
            _configuration = configuration;
            _brevoMailService = brevoMailService;
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

        }
        public async Task<bool> RegisterUser(RegisterUserDto userDto)
        {
            if (await _authRepository.UserExists(userDto.email))
            {
                return false;

            }
            
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(userDto.password);
            userDto.password = hashPassword;
            
            _otpStore[userDto.phone] = userDto;

            var otpSent = await _otpService.SendSmsAsync(userDto.phone);
            if (!otpSent)
            {
                
                _otpStore.Remove(userDto.phone);
                return false;
            }
            return true;
        }
    
        public async Task<bool> VerifyUserOtp(VerifyOtpDto verifyOtpDto)
        {
            var isValid=await _otpService.VerifyOtpAsync(verifyOtpDto);
            if (isValid)
            {
                if (_otpStore.TryGetValue(verifyOtpDto.phone, out var userDto))
                {
                    var user = new User
                    {
                        name = userDto.name,
                        email = userDto.email,
                        phone =verifyOtpDto. phone,
                        created_at = DateTime.UtcNow,
                        is_blocked = false,
                        password = userDto.password,
                        updated_at = DateTime.UtcNow,
                    };
                    await _authRepository.CreateUser(user);
                    _otpStore.Remove(verifyOtpDto.phone);
                    return true;

                }
                return false;
               


            }
            return false;
        }
        public async Task<LoginResponseDto> LoginUser(LoginUserDto userDto)
        {
            var user = await _authRepository.GetUserByEmail(userDto.email);
            if (user == null)
            {
                return new LoginResponseDto
                {
                    message = "User Not Found"
                };
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDto.password, user.password);
            if (!isPasswordValid)
            {
                return new LoginResponseDto
                {
                    message = "Invalid Email"
                };
            }
            var tokenHelper = new TokenHelper();

            var newRefreshToken = tokenHelper.GenerateRefreshTokenUser(user);

            if (string.IsNullOrEmpty(newRefreshToken))
            {
                throw new Exception("Failed to generate refresh token.");
            }

            user.refresh_token = newRefreshToken;
            user.refreshtoken_expiryDate = DateTime.UtcNow.AddDays(7);
            user.updated_at = DateTime.UtcNow;
            var tokens = new UserToken();
            var token = tokens.GenerateJwtToken(user);
            _authRepository.Update(user);
            await _authRepository.SaveChangesAsync();
            return new LoginResponseDto
            {
                id = user.id,
                name = user.name,
                token = token,
                refresh_token=newRefreshToken,
                message = "Successful"
            };
        }
       

        public async Task<string>SendResetOtp(ForgotPasswordDto forgotPasswordDto)
        {
            var user = _authRepository.GetUserByEmail(forgotPasswordDto.email);
            if (user==null)
            {
                return "User Not Found";
            }
            string otp = _brevoMailService.GenerateOtp();
            var res=await _brevoMailService.SendOtpEmailAsync(forgotPasswordDto.email, otp);
            if (res)
            {
                _emailOtpStore[forgotPasswordDto.email] = otp;
                return "otp sended";
            }
            else
            {
                return "failed";
            }
            
        }
        public bool VerifyEmailOtp(VerifyEmailOtpDto verifyEmailOtp)
        {
            var storedOtp= _emailOtpStore[verifyEmailOtp.email];
            if (storedOtp == verifyEmailOtp.otp)
            {
                _emailOtpStore.Remove(verifyEmailOtp.email);
                return true;
                
            }
            else
            {
                return false;
            }
        }
        public async Task<string> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _authRepository.GetUserByEmail(resetPasswordDto.email);
            if (user == null)
            {
                return "User Not Found";
            }
            else
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto. password);
                bool passwordUpdated = await _authRepository.UpdatePassword(user, hashedPassword);

                if (passwordUpdated)
                {
                    return "Password updated";
                }
                else
                {
                    return "updation failed";
                }
            }
        }

    }
}
