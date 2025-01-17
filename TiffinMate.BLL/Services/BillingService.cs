using Microsoft.Extensions.Options;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces;
using TiffinMate.BLL.Interfaces.OrderServiceInterface;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Interfaces.OrderInterface;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using TiffinMate.DAL.Interfaces.UserInterfaces;

namespace TiffinMate.BLL.Services
{
    public class BillingService:IBillingService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFoodItemRepository _foodItemRepository;
        private readonly IBrevoMailService _brevoMailService;
        private readonly BrevoSettings _brevoSettings;

        public BillingService(IUserRepository userRepository,ISubscriptionRepository subscriptionRepository,IFoodItemRepository foodItemRepository, IOptions<BrevoSettings> brevoSettings)
        {
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
            _foodItemRepository = foodItemRepository;
            _brevoSettings = brevoSettings.Value;

        }
        public async Task SendMonthlyBills()
        {
            try
            {
                if (DateTime.UtcNow.Day != 1)
                {
                    return;
                }
                var subscriptions = await _userRepository.GetSubscribedUsers();
                foreach (var subscription in subscriptions)
                {
                    var totalCategories = subscription.details.Count();
                    decimal monthly_amount = await _foodItemRepository.GetMonthlyTotalAmount(subscription.menu_id);
                    decimal total_amount = monthly_amount/totalCategories;

                    var invoice = await GeneratePaymentHistory(subscription, total_amount);
                    await SendBillEmail(subscription.user.email, invoice);

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<PaymentHistory> GeneratePaymentHistory(TiffinMate.DAL.Entities.OrderEntity.Subscription subscription, decimal amount)
        {
            return new PaymentHistory
            {
                user_id = subscription.user_id,
                amount = amount,
                payment_date = DateTime.UtcNow,
                subscription_id = subscription.id
                
            };
        }

        public async Task<bool> SendBillEmail(string to, PaymentHistory invoice)
        {
            string paymentLink = $"/pay/{invoice.subscription_id}";
            var emailData = new
                {
                    sender = new { email = _brevoSettings.FromEmail },
                    to = new[] { new { email = to } },
                    subject = "Monthly Bill",
                    textContent = $"Hello,\n\n" +
                    $"Your monthly bill is ready.\n\n" +
                    $"Amount Due: {invoice.amount:C}\n" +
                    $"Payment Date: {invoice.payment_date:MMMM dd, yyyy}\n\n" +
                    $"Please click the link below to view and pay your bill:\n" +
                    $"{paymentLink}\n\n" +
                    $"Thank you,\nTiffinMate"
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

