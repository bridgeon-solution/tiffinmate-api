using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sib_api_v3_sdk.Client;
using System.Net;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.OrderServiceInterface;
using TiffinMate.DAL.Entities.ProviderEntity;
using Twilio.Http;

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
        public async Task<IActionResult>Createorder( [FromBody] OrderRequestDTO orderRequestDTO)
        {
                var res = await _orderService.OrderCreate( orderRequestDTO);
                if (res==null)
                {
                    return BadRequest(new TiffinMate.API.ApiRespons.ApiResponse<string>("failure", "Addition failed", null, HttpStatusCode.BadRequest, "food is not available"));
                }
                var result = new TiffinMate.API.ApiRespons.ApiResponse<OrderResponceDto>("success", "Addition Successful", res, HttpStatusCode.OK, "");
                return Ok(result);
            
           
        }
    }
}
