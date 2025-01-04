using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.OrderServiceInterface;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Interfaces.OrderInterface;
using TiffinMate.DAL.Repositories.OrderRepository;
using static Supabase.Postgrest.Constants;

namespace TiffinMate.BLL.Services.OrderService
{
    public class OrderService :IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly AppDbContext _context;
        private readonly string _KeyId;
        private readonly string _KeySecret;
        private readonly IMapper _mapper;

        public OrderService( IOrderRepository  orderRepository,AppDbContext appDbContext,IMapper mapper) {

            _orderRepository = orderRepository;
            _context = appDbContext;
            _KeyId = Environment.GetEnvironmentVariable("RazorPay_KeyId");
            _KeySecret = Environment.GetEnvironmentVariable("RazorPay_KeySecret");
            _mapper = mapper;
        }

        //post order details
        public async Task<Guid> OrderCreate( OrderRequestDTO orderRequestDTO)
        {

            var provider = await _context.Providers.FirstOrDefaultAsync(p => p.id == orderRequestDTO.provider_id);
            if (provider == null)
            {
                throw new Exception("Provider not found.");
            }

            var dayOfWeek = DateTime.Parse(orderRequestDTO.date).DayOfWeek.ToString();
            var parsedDate = DateTime.Parse(orderRequestDTO.date);
            var isoStartDate = parsedDate.ToString("o");

            var orderId = Guid.NewGuid();

            var newOrder = new DAL.Entities.OrderEntity.Order
            {
                id = orderId,
                user_id = orderRequestDTO.user_id,
                provider_id = orderRequestDTO.provider_id,
                menu_id = orderRequestDTO.menu_id,
                start_date = isoStartDate,
                total_price=orderRequestDTO.total_price,

                //order_string = orderRequestDTO.order_string,
                //transaction_id = orderRequestDTO.transaction_string
            };

            await _context.order.AddAsync(newOrder);
            await _context.SaveChangesAsync();

            return orderId;
            
        }


        public async Task<OrderResponceDto> OrderDetailsCreate(OrderDetailsRequestDto orderDetailsRequestDto, Guid orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var categories = await _orderRepository.CreateOrder();
                var selectedCategories = categories.Where(c => orderDetailsRequestDto.categories.Contains(c.id)).ToList();

                if (!selectedCategories.Any())
                {
                    throw new Exception("No matching categories found.");
                }

                var parsedDate = DateTime.Parse(orderDetailsRequestDto.date);
                var dayOfWeek = parsedDate.DayOfWeek.ToString();

                var foodItems = await _context.FoodItems
                    .Where(f => f.menu_id == orderDetailsRequestDto.menu_id &&
                                f.provider_id == orderDetailsRequestDto.provider_id &&
                                f.day == dayOfWeek &&
                                selectedCategories.Select(c => c.id).Contains(f.category_id))
                    .ToListAsync();

                if (!foodItems.Any())
                {
                    throw new Exception("No food items found for the selected menu and categories.");
                }

                foreach (var category in selectedCategories)
                {
                    var categoryFoodItems = foodItems.Where(f => f.category_id == category.id).ToList();

                    if (!categoryFoodItems.Any())
                    {
                        continue;
                    }

                    foreach (var foodItem in categoryFoodItems)
                    {
                        var details = new OrderDetails
                        {
                            id = Guid.NewGuid(),
                            user_name = orderDetailsRequestDto.user_name,
                            address = orderDetailsRequestDto.address,
                            city = orderDetailsRequestDto.city,
                            ph_no = orderDetailsRequestDto.ph_no,
                            fooditem_name = foodItem.food_name,
                            fooditem_image = foodItem.image,
                            order_id = orderId,
                            category_id = category.id
                        };

                        await _context.orderDetails.AddAsync(details);
                    }
                }

                await _context.SaveChangesAsync();

                
                    var order = await _context.order.FirstOrDefaultAsync(o => o.id == orderId);
                    if (order != null)
                    {
                        order.payment_status = true;
                    order.order_string = orderDetailsRequestDto.order_string;
                    order.transaction_id=orderDetailsRequestDto.transaction_string;
                        _context.order.Update(order);
                        await _context.SaveChangesAsync();
                    }
                
                else
                {
                    throw new Exception("Payment failed. Cannot complete the order.");
                }

                
                await transaction.CommitAsync();

                return new OrderResponceDto
                {
                    orderdetails_id = Guid.NewGuid(),
                    order_id = orderId,
                    user_name = orderDetailsRequestDto.user_name,
                    city = orderDetailsRequestDto.city,
                    phone_number = orderDetailsRequestDto.ph_no,
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Order creation failed: {ex.Message}", ex);
            }
        }

       





        //razaorpay id create

        public async Task<string> RazorPayorderIdCreate(long price)
        {
            Dictionary<string,object> input=new Dictionary<string,object>();
            Random random = new Random();
            string TransactionId=random.Next(0,100).ToString();
            input.Add("amount", Convert.ToDecimal(price) * 100);
            input.Add("currency", "INR");
            input.Add("receipt", TransactionId);

           
            RazorpayClient client=new RazorpayClient(_KeyId, _KeySecret);
            Razorpay.Api.Order order=client.Order.Create(input);
            var OrderId = order["id"].ToString();   
            return OrderId;

        }

        //payment
        public async Task<bool> payment(RazorPayDto razorPayDto)
        {
            if (razorPayDto == null ||
                razorPayDto.razor_id == null ||
                razorPayDto.razor_orderid == null ||
                razorPayDto.razor_sign == null)
                return false;

            RazorpayClient razorpay = new RazorpayClient(_KeyId, _KeySecret);
            Dictionary<string, string> attributes = [];
            attributes.Add("Razorpay_paymentId", razorPayDto.razor_id);
            attributes.Add("Razorpay_orderId", razorPayDto.razor_orderid);
            attributes.Add("Razorpay_signature", razorPayDto.razor_sign);
            Utils.verifyPaymentLinkSignature(attributes);
            return true;

        }

        public async Task<OrderRequestDTO> OrderGetedByOrderId(Guid OrderId)
        {
            var order = await _orderRepository.GetOrders(OrderId);
            return _mapper.Map<OrderRequestDTO>(order);
        }

        //orders
        public async Task<List<AllOrderByProviderDto>> OrderLists(Guid ProviderId, int page, int pageSize, string search = null)

        {
            var orders = (await _orderRepository.GetOrdersByProvider(ProviderId)).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                orders = orders
            .Where(u => u.user.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        u.details.Any(d => d.fooditem_name.Contains(search, StringComparison.OrdinalIgnoreCase))).ToList();

            }
            var totalCount = orders.Count;

            var Allorder = orders.SelectMany(o => o.details.Select(d => new GetOrderDetailsDto
            {
                user_name = d.user_name,
                address = d.address,
                city = d.city,
                ph_no = d.ph_no,
                fooditem_name = d.fooditem_name,

            })).ToList();
            var pagedOrders = Allorder.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new AllOrderByProviderDto
            {
                TotalCount = totalCount,
                Allorders = pagedOrders
            };
            return new List<AllOrderByProviderDto> { result };
        }
        //users
        public async Task<List<AllUserOutputDto>> UsersLists(Guid ProviderId, int page, int pageSize, string search = null)

        {
            var orders = (await _orderRepository.GetOrdersByProvider(ProviderId)).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                orders = orders
            .Where(u => u.user.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        u.user.email.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            }
            var totalCount = orders.Count;



            var Allusers = orders.GroupBy(o => o.user_id).Select(g => g.First()).Select(o => new AllUsersDto
            {
                user_name = o.user.name,
                address = o.user.address,
                city = o.user.city,
                ph_no = o.user.phone,
                image = o.user.image,
                email = o.user.email
            }).ToList();
            var pagedOrders = Allusers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new AllUserOutputDto
            {
                TotalCount = totalCount,
                AllUsers = pagedOrders
            };
            return new List<AllUserOutputDto> { result };
        }


        public async Task<AllUserOutputDto> GetUserOrders(int page, int pageSize, string search = null, string filter = null)
        {
            
            var result = await _context.order
                .Include(o => o.details)
                .Where(o => o.payment_status) 
                .SelectMany(order => order.details, (order, detail) => new AllUsersDto
                {
                    user_name = detail.user_name,
                    ph_no = detail.ph_no,
                    city = detail.city,
                    total_price = order.total_price,
                    order_id = order.id,
                    start_date = DateTime.Parse(order.start_date)
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(u =>
                    u.user_name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    u.city.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (filter.Equals("newest", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(u => u.start_date).ToList(); 
                }
                else if (filter.Equals("oldest", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(u => u.start_date).ToList(); 
                }
            }

          
            var total = result.Count;

            var pagedUsers = result
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var newResult = new AllUserOutputDto
            {
                TotalCount = total,
                AllUsers = pagedUsers,
            };

            return newResult;
        }

    }
}
