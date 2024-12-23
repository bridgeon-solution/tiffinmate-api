using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces.ProviderVerification;
using Microsoft.Extensions.Options;
using TiffinMate.DAL.Entities;
using Microsoft.Extensions.Logging;

namespace TiffinMate.BLL.Services.ProviderVerification
{
    public class ProviderBrevoMailService : IProviderBrevoMailService
    {
        private readonly BrevoSettings _brevoSettings;
        private readonly ILogger<ProviderBrevoMailService> _logger;
        public ProviderBrevoMailService(IOptions<BrevoSettings> brevoSettings, ILogger<ProviderBrevoMailService> logger)
        {
            _brevoSettings = brevoSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendOtpEmailAsync(string to, string otp)
        {
            var emailData = new
            {
                sender = new { email = _brevoSettings.FromEmail },
                to = new[] { new { email = to } },
                subject = "Your password",
                textContent = $"Hello,\n\nYour Password is: {otp}\n\nThis password is valid for 10 minutes.\n\nThank you,\nTiffinMate "
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("api-key", _brevoSettings.ApiKey);
                    var jsonData = JsonSerializer.Serialize(emailData);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(_brevoSettings.ApiUrl, content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Email sending failed: {response.StatusCode} - {responseBody}");
                        _logger.LogError("Failed to send OTP email. Status Code: {StatusCode}, Response: {ResponseBody}", response.StatusCode, responseBody);
                        return false;
                    }

                    Console.WriteLine($"Email sent successfully to {to}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending OTP email to {Recipient}", to);
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Rejectmail(string to)
        {
            var emailData = new
            {
                sender = new { email = _brevoSettings.FromEmail },
                to = new[] { new { email = to } },
                subject = "Your password",
                textContent = $"Hello,\n\nYour verification is rejected \n\nThank you,\nTiffinMate "
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("api-key", _brevoSettings.ApiKey);
                    var jsonData = JsonSerializer.Serialize(emailData);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(_brevoSettings.ApiUrl, content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Email sending failed: {response.StatusCode} - {responseBody}");
                        _logger.LogError("Failed to send  email. Status Code: {StatusCode}, Response: {ResponseBody}", response.StatusCode, responseBody);
                        return false;
                    }

                    Console.WriteLine($"Email sent successfully to {to}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending OTP email to {Recipient}", to);
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendOtpForgetPassword(string to, string otp)
        {
            var emailData = new
            {
                sender = new { email = _brevoSettings.FromEmail },
                to = new[] { new { email = to } },
                subject = "reset password",
                textContent = $"Hello,\n\nYour reset password otp is: {otp}\n\nThis otp is valid for 10 minutes.\n\nThank you,\nTiffinMate "
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("api-key", _brevoSettings.ApiKey);
                    var jsonData = JsonSerializer.Serialize(emailData);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(_brevoSettings.ApiUrl, content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {

                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
