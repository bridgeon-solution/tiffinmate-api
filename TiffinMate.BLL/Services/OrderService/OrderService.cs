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

namespace TiffinMate.BLL.Services.OrderService
{
    public class OrderService :IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly AppDbContext _context;
        private readonly string _KeyId;
        private readonly string _KeySecret;

        public OrderService( IOrderRepository  orderRepository,AppDbContext appDbContext) {

            _orderRepository = orderRepository;
            _context = appDbContext;
            _KeyId = Environment.GetEnvironmentVariable("RazorPay_KeyId");
            _KeySecret = Environment.GetEnvironmentVariable("RazorPay_KeySecret");
        }

        //post order details
        public async Task<OrderResponceDto> OrderCreate( OrderRequestDTO orderRequestDTO)
        {
            
            var categories = await _orderRepository.CreateOrder();
            var selectedCategories = categories.Where(c => orderRequestDTO.categories.Contains(c.id)).ToList();

            if (!selectedCategories.Any())
            {
                throw new Exception("No matching categories found.");
            }

            var provider = await _context.Providers.FirstOrDefaultAsync(p => p.id == orderRequestDTO.provider_id);
            if (provider == null)
            {
                throw new Exception("Provider not found.");
            }

            var dayOfWeek = DateTime.Parse(orderRequestDTO.date).DayOfWeek.ToString();
            var parsedDate = DateTime.Parse(orderRequestDTO.date);
            var isoStartDate = parsedDate.ToString("o");

        
            var foodItems = await _context.FoodItems
                .Where(f => f.menu_id == orderRequestDTO.menu_id &&
                            f.provider_id == orderRequestDTO.provider_id &&
                            f.day == dayOfWeek &&
                            selectedCategories.Select(c => c.id).Contains(f.category_id))
                .ToListAsync();

            if (!foodItems.Any())
            {
                throw new Exception("No food items found for the selected menu and categories.");
            }

            var orderId = Guid.NewGuid();

            var newOrder = new DAL.Entities.OrderEntity.Order
            {
                id = orderId,
                user_id = orderRequestDTO.user_id,
                provider_id = orderRequestDTO.provider_id,
                menu_id = orderRequestDTO.menu_id,
                start_date = orderRequestDTO.date,
                order_string = orderRequestDTO.order_string,
                transaction_id=orderRequestDTO.transacttion_id
            };

            await _context.order.AddAsync(newOrder);

           
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
                        user_name = orderRequestDTO.user_name,
                        address = orderRequestDTO.address,
                        city = orderRequestDTO.city,
                        ph_no = orderRequestDTO.ph_no,
                        fooditem_name = foodItem.food_name, 
                        fooditem_image = foodItem.image,   
                        order_id = orderId,               
                        category_id = category.id        
                    };

                   
                    await _context.orderDetails.AddAsync(details);

                    
                }
            }

            
            await _context.SaveChangesAsync();

            return new OrderResponceDto
            {
                OrderId = newOrder.id,
                UserId = newOrder.user_id,
                ProviderId = newOrder.provider_id,
                MenuId = newOrder.menu_id,
                StartDate = newOrder.start_date,
                
            };
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
        public async Task <bool> payment(RazorPayDto razorPayDto)
        {
            if (razorPayDto == null ||
                razorPayDto.razor_id == null ||
                razorPayDto.razor_orderid == null ||
                razorPayDto.razor_sign == null)
                return false;
           
                RazorpayClient razorpay = new RazorpayClient(_KeyId,_KeySecret);
                Dictionary<string, string> attributes = [];
                attributes.Add("Razorpay_paymentId", razorPayDto.razor_id);
                attributes.Add("Razorpay_orderId", razorPayDto.razor_orderid);
                attributes.Add("Razorpay_signature", razorPayDto.razor_sign);
                Utils.verifyPaymentLinkSignature(attributes);
                return true;

            
               
        }


    }
}
