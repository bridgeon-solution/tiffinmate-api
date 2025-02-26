using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Interfaces.OrderInterface
{
    public interface ISubscriptionRepository
    {
        Task<List<Categories>> CreateSubscription();
        Task<Subscription> GetSubscriptionByid(Guid OrderId);
        Task<List<Subscription>> GetProviderSubscription(Guid providerId);
        Task<string> categoryById(Guid id);
        Task<List<Subscription>> GetUserSubscriptions(Guid userId);
        Task<List<Subscription>> GetSubscribedUsers();
        Task UpdateSubscriptionAsync(Subscription subscription);
        Task UpdatePaymentHistoryAsync(PaymentHistory paymentHistory);
        Task<List<PaymentHistory>> GetPaymentHistory(Guid?id);
        Task<List<Subscription>> GetAllSubscription();


    }
}
