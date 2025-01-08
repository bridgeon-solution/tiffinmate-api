using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.OrderDTOs;

namespace TiffinMate.BLL.Interfaces.OrderServiceInterface
{
    public interface ISubscriptionService
    {
        Task<Guid> SubscriptionCreate(OrderRequestDTO orderRequestDTO);
        Task<OrderResponceDto> SubscriptionDetailsCreate(OrderDetailsRequestDto orderDetailsRequestDto, Guid orderId);
        Task<OrderRequestDTO> SubscriptionGetedById(Guid OrderId);
        Task<List<AllSubByProviderDto>> SubscriptionLists(Guid ProviderId, int page, int pageSize, string search = null, string filter = null);
        Task<string> categoryById(Guid id);
        Task<AllOrderDTO> GetSubscribtionOrders(int page, int pageSize, string search = null, string filter = null);
    }
}
