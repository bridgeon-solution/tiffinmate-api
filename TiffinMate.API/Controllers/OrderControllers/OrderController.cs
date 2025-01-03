using Asp.Versioning;
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

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {

            _orderService = orderService;

        }

        [HttpPost]
        public async Task<IActionResult> Createorder([FromBody] OrderRequestDTO orderRequestDTO)
        {
            try
            {
                var res = await _orderService.OrderCreate(orderRequestDTO);
                if (res == null)
                {
                    return BadRequest(new TiffinMate.API.ApiRespons.ApiResponse<string>("failure", "order failed", null, HttpStatusCode.BadRequest, "order failed"));
                }
                var result = new TiffinMate.API.ApiRespons.ApiResponse<Guid>("success", "Addition Successful", res, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }




        }

        [HttpPost("details")]
        public async Task<IActionResult> CreateOrderDetails(OrderDetailsRequestDto orderDetailrequest, Guid orderid)
        {
            try
            {
                var res = await _orderService.OrderDetailsCreate(orderDetailrequest, orderid);
                if (res == null)
                {
                    return BadRequest(new TiffinMate.API.ApiRespons.ApiResponse<string>("failure", "Addition failed", null, HttpStatusCode.BadRequest, "order details are necessary"));
                }
                var result = new TiffinMate.API.ApiRespons.ApiResponse<OrderResponceDto>("success", "order details added Successfully", res, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }

        [HttpPost("razorpay_order")]
        public async Task<IActionResult> RazorPayidCreate(long price)
        {
            try

            {
                var res = await _orderService.RazorPayorderIdCreate(price);

                if (price <= 0 || price > 100000)
                {
                    return BadRequest(new TiffinMate.API.ApiRespons.ApiResponse<string>("failure", "razor_pay id create failed", null, HttpStatusCode.BadRequest, "invalid amount"));
                }

                var result = new TiffinMate.API.ApiRespons.ApiResponse<string>("succesfull", "razor_pay id created succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment(RazorPayDto razorPayDto)
        {
            try
            {
                var res = await _orderService.payment(razorPayDto);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<bool>("succesfull", "payment succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }

        [HttpGet("{orderid}")]
        public async Task<IActionResult> GetOrderById(Guid orderid)
        {
            try
            {
                var res = await _orderService.OrderGetedByOrderId(orderid);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<OrderRequestDTO>("succesfull", "Order details getted succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
    }
}
