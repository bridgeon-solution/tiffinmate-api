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
        public async Task<Subscription> GetSubscriptionByid(Guid OrderId)
        {
            return await _context.subscriptions.FirstOrDefaultAsync(o => o.id==OrderId);
        }
    }
}
