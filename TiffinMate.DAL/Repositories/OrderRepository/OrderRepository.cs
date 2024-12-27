using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.OrderInterface;

namespace TiffinMate.DAL.Repositories.OrderRepository
{
    public class OrderRepository : IorderInterface
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task <List<Categories>> CreateOrder()
        {
            return await _context.Categories.ToListAsync();
        }
    }
};
