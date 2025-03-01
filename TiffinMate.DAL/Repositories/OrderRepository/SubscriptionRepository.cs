using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.OrderInterface;

namespace TiffinMate.DAL.Repositories.OrderRepository
{
    public class SubscriptionRepository:ISubscriptionRepository
    {
        private readonly AppDbContext _context;
        public SubscriptionRepository(AppDbContext appDbContext) { 
            _context = appDbContext;
        }
        public async Task<List<Categories>> CreateSubscription()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Subscription> GetSubscriptionByid(Guid? OrderId)
        {
            return await _context.subscriptions.FirstOrDefaultAsync(o => o.id == OrderId);
        }
public async Task<List<Subscription>> GetProviderSubscription(Guid providerId)
        {
            return await _context.subscriptions
         .Where(o => o.provider_id == providerId)
         .Include(p => p.provider).ThenInclude(o => o.menus)
         .Include(f => f.provider).ThenInclude(f => f.food_items)
         .Include(p => p.details).ThenInclude(o => o.Category)
         .Include(u => u.user)
         .ToListAsync();
        }        
        public async Task<string> categoryById(Guid id)
        {
            var category = await _context.Categories
        .FirstOrDefaultAsync(o => o.id == id);
            return category?.category_name;

        }
        public async Task<List<Subscription>> GetUserSubscriptions(Guid userId)
        {
            return await _context.subscriptions
                .Where(s => s.user_id == userId)
                .ToListAsync();
        }
        public async Task<List<Subscription>> GetSubscribedUsers()
        {
            return await _context.subscriptions
                .Include(s => s.details)
                .Include(s => s.user)
                .Where(s => s.is_active)
                .ToListAsync() ?? new List<Subscription>();
        }
        public async Task UpdateSubscriptionAsync(Subscription subscription)
        {
            _context.subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePaymentHistoryAsync(PaymentHistory paymentHistory)
        {
             _context.paymentHistory.Update(paymentHistory);
            await _context.SaveChangesAsync();
        }
        public async Task<List<PaymentHistory>> GetPaymentHistory(Guid? id)
        {
            if (id.HasValue)
            {
                return await _context.paymentHistory
                                     .Where(e => e.id == id.Value)
                                     .ToListAsync();
            }
            return await _context.paymentHistory.ToListAsync();
        }

    }
}
