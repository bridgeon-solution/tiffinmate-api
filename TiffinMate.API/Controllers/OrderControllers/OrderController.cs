using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.OrderServiceInterface;

namespace TiffinMate.API.Controllers.OrderControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) {
           
            _orderService = orderService;

        }

        [HttpPost]
        public async Task<IActionResult>createorder([FromBody] PlanRequest planreqest, [FromQuery] Guid providerid, [FromQuery] Guid menuid, Guid userid, [FromQuery] OrderRequestDTO orderRequestDTO)
        {
            var res = await _orderService.OrderCreate(planreqest, providerid, menuid, userid, orderRequestDTO);
            return Ok(res);
        }
    }
}
