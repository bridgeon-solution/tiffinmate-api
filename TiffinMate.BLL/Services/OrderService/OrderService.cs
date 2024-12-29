using Microsoft.EntityFrameworkCore;
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
        public OrderService( IOrderRepository  orderRepository,AppDbContext appDbContext) {

            _orderRepository = orderRepository;
            _context = appDbContext;
        }
        public async Task<bool> OrderCreate(PlanRequest planreqest, Guid providerid, Guid menuid, Guid userid, OrderRequestDTO orderRequestDTO)
        {
            
            var categories = await _orderRepository.CreateOrder();
            var selectedCategories = categories.Where(c => planreqest.categories.Contains(c.id)).ToList();

            if (!selectedCategories.Any())
            {
                throw new Exception("No matching categories found.");
            }

            var provider = await _context.Providers.FirstOrDefaultAsync(p => p.id == providerid);
            if (provider == null)
            {
                throw new Exception("Provider not found.");
            }

            var dayOfWeek = DateTime.Parse(planreqest.date).DayOfWeek.ToString();
            var parsedDate = DateTime.Parse(planreqest.date);
            var isoStartDate = parsedDate.ToString("o");

        
            var foodItems = await _context.FoodItems
                .Where(f => f.menu_id == menuid &&
                            f.provider_id == providerid &&
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
                user_id = userid,
                provider_id = providerid,
                menu_id = menuid,
                start_date = isoStartDate,
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
                        fooditem_name = foodItem.food_name, 
                        fooditem_image = foodItem.image,   
                        order_id = orderId,               
                        category_id = category.id        
                    };

                   
                    await _context.orderDetails.AddAsync(details);
                }
            }

            
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
