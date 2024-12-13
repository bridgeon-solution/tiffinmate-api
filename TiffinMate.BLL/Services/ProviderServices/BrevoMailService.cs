﻿using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces.ProviderVerification;
using Microsoft.Extensions.Options;
using TiffinMate.DAL.Entities;

namespace TiffinMate.BLL.Services.ProviderVerification
{
    public class BrevoMailService : IBrevoMailService
    {
        private readonly BrevoSettings _brevoSettings;

        public BrevoMailService(IOptions<BrevoSettings> brevoSettings)
        {
            _brevoSettings = brevoSettings.Value;
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
                        return false;
                    }

                    Console.WriteLine($"Email sent successfully to {to}");
                    return true;
                }
            }
            catch (Exception ex)
            {
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
                subject = "Reject Verification",
                textContent = $"Hello,\n\nYour Verification is rejected.\n\nThank you,\nTiffinMate"
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
                        return false;
                    }

                    Console.WriteLine($"Email sent successfully to {to}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
