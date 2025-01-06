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
using static Supabase.Gotrue.Constants;

namespace TiffinMate.DAL.Repositories.OrderRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Categories>> CreateOrder()
        {
            return await _context.Categories.ToListAsync();
        }


      



        public async Task<List<Order>> GetOrdersByProvider(Guid providerId)
        {
            return await _context.order.Where(u => u.provider_id == providerId).Include(o => o.details).Include(o => o.user).Include(o => o.provider).ToListAsync();
        }

       
    }
};
