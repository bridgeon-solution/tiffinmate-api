using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Interfaces.OrderInterface;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using TiffinMate.DAL.Interfaces.UserInterfaces;

namespace TiffinMate.BLL.Services
{
    public class BillingService:IBillingService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IFoodItemRepository _foodItemRepository;
        private readonly IBrevoMailService _brevoMailService;
        private readonly BrevoSettings _brevoSettings;
        private readonly AppDbContext _context;

        public BillingService(ISubscriptionRepository subscriptionRepository,IFoodItemRepository foodItemRepository, IOptions<BrevoSettings> brevoSettings, AppDbContext context)
        {
            _subscriptionRepository = subscriptionRepository;
            _foodItemRepository = foodItemRepository;
            _brevoSettings = brevoSettings.Value;
            _context = context;

        }
        public async Task SendMonthlyBills()
        {
            try
            {
                if (DateTime.UtcNow.Day != 1)
                {
                    return;
                }
                var subscriptions = await _subscriptionRepository.GetSubscribedUsers();
                if (!subscriptions.Any())
                {
                    return;
                }
                foreach (var subscription in subscriptions)
                {
                    var totalCategories = subscription.details.Count();
                    decimal monthlyAmount = await _foodItemRepository.GetMonthlyTotalAmount(subscription.menu_id);
                    if (monthlyAmount == 0)
                    {
                        continue;
                    }
                    decimal totalAmount = monthlyAmount/totalCategories;
                    var paymentHistory = await SavePaymentHistory(subscription, totalAmount);
                    await SendBillEmail(subscription.user.email, paymentHistory);

                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async Task<PaymentHistory> SavePaymentHistory(TiffinMate.DAL.Entities.OrderEntity.Subscription subscription, decimal amount)
        {
            var paymentHistory = new PaymentHistory
            {

                subscription_id = subscription.id,
                amount = amount,
                payment_date = DateTime.UtcNow,
                user_id = subscription.user_id,

            };
            await _context.paymentHistory.AddAsync(paymentHistory);
            await _context.SaveChangesAsync();
            return paymentHistory;
        }

        public async Task<bool> SendBillEmail(string to, PaymentHistory paymentHistory)
        {
            string paymentLink = $"http://localhost:5174/?paymentId={paymentHistory.id}";
            var emailData = new
                {
                    sender = new { email = _brevoSettings.FromEmail },
                    to = new[] { new { email = to } },
                    subject = "Monthly Bill",
                    textContent = $"Hello,\n\n" +
                    $"Your monthly bill is ready.\n\n" +
                    $"Amount Due: {paymentHistory.amount:C}\n" +
                    $"Please click the link below to view and pay your bill:\n" +
                     $"Payment ID: {paymentHistory.id}\n\n" +
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

