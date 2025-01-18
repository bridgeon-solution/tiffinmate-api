using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.Interfaces.OrderServiceInterface;
using TiffinMate.BLL.Services.OrderService;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.API.Controllers.OrderControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> Createorder([FromBody] OrderRequestDTO orderRequestDTO)
        {
            try
            {
                var res = await _subscriptionService.SubscriptionCreate(orderRequestDTO);
                if (res == null)
                {
                    return Ok(new TiffinMate.API.ApiRespons.ApiResponse<string>("failure", "order failed", null, HttpStatusCode.BadRequest, "order failed"));
                }
                var result = new TiffinMate.API.ApiRespons.ApiResponse<Guid>("success", "subscription created Successful", res, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
        [HttpPost("details")]
        public async Task<IActionResult> CreateOrderDetails(OrderDetailsRequestDto orderDetailrequest, Guid subscriptionid)
        {
            try
            {
                var res = await _subscriptionService.SubscriptionDetailsCreate(orderDetailrequest, subscriptionid);
                if (res == null)
                {
                    return BadRequest(new TiffinMate.API.ApiRespons.ApiResponse<string>("failure", "failed subscription detail creation", null, HttpStatusCode.BadRequest, "subscription details are necessary"));
                }
                var result = new TiffinMate.API.ApiRespons.ApiResponse<OrderResponceDto>("success", "subsription details added Successfully", res, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetOrderById(Guid subscriptionid)
        {
            try
            {
                var res = await _subscriptionService.SubscriptionGetedById(subscriptionid);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<OrderRequestDTO>("succesfull", "subscription details getted succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
        [HttpGet("{providerId}")]
        public async Task<IActionResult> AllOrders(Guid providerId, int page = 1, int pageSize = 10, string search = null, string filter = null)
        {
            try
            {
                var res = await _subscriptionService.SubscriptionLists(providerId, page, pageSize, search, filter);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<List<AllSubByProviderDto>>("succesfull", "Getting Orders succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }

        }

        [HttpGet]
        public async Task<IActionResult> GetSubscriptionOrder(int page, int pageSize, string search = null, string filter = null)
        {
            try
            {
                var res = await _subscriptionService.GetSubscribtionOrders(page, pageSize, search, filter);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<AllOrderDTO>("succesfull", "Subscription details getted succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
        [HttpGet("all/{providerId}")]
        public async Task<IActionResult> AllOrders(Guid providerId, int page = 1, int pageSize = 10, string search = null, string filter = null, string toggle = null)
        {
            try
            {
                var res = await _subscriptionService.SubscriptionLists(providerId, page, pageSize, search, filter, toggle);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<List<AllSubByProviderDto>>("succesfull", "Getting Orders succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }

        }
        [HttpGet("payment-history")]
        public async Task<IActionResult> GetPaymentHistory(Guid?id)
        {
            try
            {
                var res = await _subscriptionService.GetPaymentHistory(id);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<List<PaymentHistory>>("succesfull", "payment history getted succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
        [HttpPut("subscription")]
        public async Task<IActionResult> UpdateSubscription(PaymentHistoryRequestDto requestDto)
        {
            try
            {
                var res = await _subscriptionService.HandleSubscription(requestDto);
                if (!res)
                {
                    return BadRequest(new TiffinMate.API.ApiRespons.ApiResponse<string>("failure", "failed handle subscription", null, HttpStatusCode.BadRequest, "payment details are necessary"));
                }
                var result = new TiffinMate.API.ApiRespons.ApiResponse<bool>("success", "subscription updated successfully", res, HttpStatusCode.OK, "");
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
