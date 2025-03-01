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

        //all orders of provider
        [HttpGet("provider/{providerId}/")]
        public async Task<IActionResult> AllOrders(Guid providerId, int page = 1, int pageSize = 10, string search = null, string? filter = null)
        {
            try
            {
                var res = await _orderService.OrderLists(providerId, page, pageSize, search, filter);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<List<AllOrderByProviderDto>>("succesfull", "Getting Orders succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
     
        //order of user
        [HttpGet("users/{providerId}")]
        public async Task<IActionResult> UserOrders(Guid providerId, Guid UserId, int page = 1, int pageSize = 10, string search = null)
        {
            try
            {
                var res = await _orderService.OrdersOfUsers(providerId, UserId, page, pageSize, search);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<List<AllOrderByProviderDto>>("succesfull", "Getting users Orders succesfully", res, HttpStatusCode.OK, "");
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
                var result = new TiffinMate.API.ApiRespons.ApiResponse<OrderDetailsResponseDTO>("succesfull", "Order details getted succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersOrder(int page, int pageSize, string search = null, string filter = null, Guid? userId = null)
        {
            try
            {
                var res = await _orderService.GetUserOrders(page, pageSize, search, filter, userId);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<AllOrderDTO>("succesfull", "Order details getted succesfully", res, HttpStatusCode.OK, "");
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
