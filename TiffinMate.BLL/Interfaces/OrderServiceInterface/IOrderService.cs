using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;

namespace TiffinMate.BLL.Interfaces.OrderServiceInterface
{
   public interface IOrderService
    {
        Task<Guid> OrderCreate( OrderRequestDTO orderRequestDTO);
        Task<OrderResponceDto> OrderDetailsCreate(OrderDetailsRequestDto orderDetailsRequestDto, Guid orderId); 
        Task<string> RazorPayorderIdCreate(long price);
        Task<bool> payment(RazorPayDto razorPayDto);
        Task<OrderDetailsResponseDTO> OrderGetedByOrderId(Guid OrderId);
        Task<List<AllOrderByProviderDto>> OrderLists(Guid ProviderId, int page, int pageSize, string search = null);
        Task<List<AllUserOutputDto>> UsersLists(Guid ProviderId, int page, int pageSize, string search = null);
        Task<AllUserOutputDto> GetUserOrders(int page, int pageSize, string search = null, string filter = null);

    }
}
