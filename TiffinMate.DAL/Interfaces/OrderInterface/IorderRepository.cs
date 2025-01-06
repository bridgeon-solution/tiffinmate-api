using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Interfaces.OrderInterface
{
    public interface IOrderRepository
    {
        public Task<List<Categories>> CreateOrder();

        Task<List<Order>> GetOrdersByProvider(Guid providerId);
    }
}
