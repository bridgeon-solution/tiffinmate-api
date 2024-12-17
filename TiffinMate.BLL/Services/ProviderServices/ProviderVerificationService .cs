using System;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces.ProviderVerification;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;

namespace TiffinMate.BLL.Services.ProviderVerification
{
    public class ProviderVerificationService : IProviderVerificationService
    {
        private readonly IProviderBrevoMailService _mailService;
        private readonly IProviderRepository _providerRepository;

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
                provider.password = otp;
                provider.updated_at= DateTime.UtcNow;
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
                await _providerRepository.Remove(providerId);
                await _providerRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating while removing provider: {ex.Message}");
                return false;
            }

        }


        private string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); 
        }
    }
}
