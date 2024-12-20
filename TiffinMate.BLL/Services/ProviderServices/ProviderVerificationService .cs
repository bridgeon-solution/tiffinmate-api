using System;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.ProviderVerification;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;

namespace TiffinMate.BLL.Services.ProviderVerification
{
    public class ProviderVerificationService : IProviderVerificationService
    {
        private readonly IProviderBrevoMailService _mailService;
        private readonly IProviderRepository _providerRepository;
        private static Dictionary<string, ProviderLoginDTO> _otpStore = new Dictionary<string, ProviderLoginDTO>();
        private static Dictionary<string, string> _emailOtpStore = new Dictionary<string, string>();
        public ProviderVerificationService(IProviderBrevoMailService mailService, IProviderRepository providerRepository)
        {
            _mailService = mailService;
            _providerRepository = providerRepository;
        }

        public async Task<bool> SendPassword(Guid providerId)
        {
           
            var provider = await _providerRepository.GetProviderById(providerId);
            if (provider == null)
            {
                Console.WriteLine($"Provider with ID {providerId} not found.");
                return false;
            }

       
            var otp = GenerateOtp();

            var emailSent = await _mailService.SendOtpEmailAsync(provider.email, otp);
            if (!emailSent)
            {
                Console.WriteLine("Failed to send OTP email.");
                return false;
            }
            try
            {
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(otp);
                provider.password = hashPassword;
                provider.updated_at = DateTime.UtcNow;
                provider.verification_status = "approved";
                _providerRepository.Update(provider);
               await _providerRepository.SaveChangesAsync();
                Console.WriteLine($"Password updated for provider {providerId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating provider password: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> RemoveData(Guid providerId)
        {

            var provider = await _providerRepository.GetProviderById(providerId);
            if (provider == null)
            {
                Console.WriteLine($"Provider with ID {providerId} not found.");
                return false;
            }
            var emailSent = await _mailService.Rejectmail(provider.email);
            if (!emailSent)
            {
                Console.WriteLine("Failed to send password.");
                return false;
            }
            try
            {
                provider.updated_at = DateTime.UtcNow;
                provider.is_delete = true;
                 _providerRepository.Update(provider);
                await _providerRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating while removing provider: {ex.Message}");
                return false;
            }

        }
        public async Task<string> SendResetOtp(ForgotPasswordDto forgotPasswordDto)
        {
            var user = _providerRepository.GetUserByEmail(forgotPasswordDto.email);
            if (user == null)
            {
                return "User Not Found";
            }
            string otp = GenerateOtp();
            var res = await _mailService.SendOtpForgetPassword(forgotPasswordDto.email, otp);
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
            var storedOtp = _emailOtpStore[verifyEmailOtp.email];
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
            var provider = await _providerRepository.GetUserByEmail(resetPasswordDto.email);
            if (provider == null)
            {
                return "User Not Found";
            }
            else
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.password);
                bool passwordUpdated = await _providerRepository.UpdatePassword(provider, hashedPassword);

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
        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); 
        }
    }
}
