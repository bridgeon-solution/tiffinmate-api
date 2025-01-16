using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces;
using TiffinMate.BLL.Interfaces.OrderServiceInterface;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Interfaces.OrderInterface;
using TiffinMate.DAL.Interfaces.UserInterfaces;

namespace TiffinMate.BLL.Services
{
    public class BillingService:IBillingService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ISubscriptionService _subscriptionService;

        public BillingService(IUserRepository userRepository,ISubscriptionRepository subscriptionRepository,ISubscriptionService subscriptionService)
        {
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
            _subscriptionService = subscriptionService;
        }
        public async Task SendMonthlyBills()
        {
            try
            {
                if (DateTime.UtcNow.Day != 1)
                {
                    return;
                }
                var users = await _userRepository.GetSubscribedUsers();
                foreach (var user in users)
                {
                    var billAmount = await CalculateUserBill(user);

                    // Generate an invoice
                    var invoice = await GenerateInvoice(user, billAmount);

                    // Send email with the invoice
                    await _subscriptionService.SendBillEmail(user.email, invoice);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private async Task<decimal> CalculateUserBill(User user)
        {
            // Fetch subscriptions for the user
            var subscriptions = await _subscriptionRepository.GetUserSubscriptions(user.id);

            // Calculate the total amount
            return subscriptions.Sum(s => s.total_price);
        }

        private async Task<Invoice> GenerateInvoice(User user, decimal amount)
        {
            // Create invoice object
            return new Invoice
            {
                UserId = user.id,
                Amount = amount,
                InvoiceDate = DateTime.UtcNow,
                InvoiceDetails = $"Invoice for {DateTime.UtcNow:MMMM yyyy}"
            };
        }

    }
}
