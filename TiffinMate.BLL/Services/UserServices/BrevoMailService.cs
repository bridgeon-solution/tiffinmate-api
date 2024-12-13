using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TiffinMate.BLL.Interfaces.UserInterfaces;

namespace TiffinMate.BLL.Services.UserServices
{
    public class BrevoMailService:IBrevoMailService
    {
        private readonly BrevoSettings _brevoSettings;

        public BrevoMailService(IOptions<BrevoSettings> brevoSettings)
        {
            _brevoSettings = brevoSettings.Value;
            
        }

        public string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        public async Task<bool> SendOtpEmailAsync(string to, string otp)
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
