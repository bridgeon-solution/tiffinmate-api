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
        Task<bool> OrderCreate(PlanRequest planreqest, Guid providerid, Guid menuid, Guid userid, OrderRequestDTO orderRequestDTO);
    }
}
