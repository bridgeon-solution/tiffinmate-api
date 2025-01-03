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
    }
}
