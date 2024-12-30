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
        Task<OrderResponceDto> OrderCreate( OrderRequestDTO orderRequestDTO);

        Task<string> RazorPayorderIdCreate(long price);
        Task<bool> payment(RazorPayDto razorPayDto);

    }
}
