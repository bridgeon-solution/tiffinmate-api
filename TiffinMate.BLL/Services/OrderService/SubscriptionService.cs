using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.Interfaces.OrderServiceInterface;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Interfaces.OrderInterface;
using TiffinMate.DAL.Repositories.OrderRepository;

namespace TiffinMate.BLL.Services.OrderService
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SubscriptionService(ISubscriptionRepository subscription, AppDbContext appDbContext,IMapper mapper)
        {
            _subscriptionRepository = subscription;
            _context = appDbContext;
            _mapper = mapper;

        }
        public async Task<Guid> SubscriptionCreate(OrderRequestDTO orderRequestDTO)
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
            var newSubscription = new DAL.Entities.OrderEntity.Subscription
            {
                id = orderId,
                user_id = orderRequestDTO.user_id,
                provider_id = orderRequestDTO.provider_id,
                menu_id = orderRequestDTO.menu_id,
                start_date = isoStartDate,
                total_price = orderRequestDTO.total_price,
            };
            await _context.subscriptions.AddAsync(newSubscription);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while saving changes: {ex.InnerException?.Message}", ex);
            }

            return orderId;

        }

        //Subscription details adding
        public async Task<OrderResponceDto> SubscriptionDetailsCreate(OrderDetailsRequestDto orderDetailsRequestDto, Guid orderId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var categories = await _subscriptionRepository.CreateSubscription();
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
                        var details = new SubscriptionDetails
                        {
                            id = Guid.NewGuid(),
                            user_name = orderDetailsRequestDto.user_name,
                            address = orderDetailsRequestDto.address,
                            city = orderDetailsRequestDto.city,
                            ph_no = orderDetailsRequestDto.ph_no,
                            
                            subscription_id = orderId,
                            category_id = category.id
                        };

                        await _context.subscriptionDetails.AddAsync(details);
                    }
                }

                await _context.SaveChangesAsync();


                var order = await _context.subscriptions.FirstOrDefaultAsync(o => o.id == orderId);
                if (order != null)
                {
                    order.payment_status = true;
                    order.order_string = orderDetailsRequestDto.order_string;
                    order.transaction_id = orderDetailsRequestDto.transaction_string;
                    _context.subscriptions.Update(order);
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

        public async Task<OrderRequestDTO> SubscriptionGetedById(Guid OrderId)
        {
            var order = await _subscriptionRepository.GetSubscriptionByid(OrderId);
            return _mapper.Map<OrderRequestDTO>(order);
        }
        public async Task<List<AllSubByProviderDto>> SubscriptionLists(Guid ProviderId, int page, int pageSize, string search = null, string filter = null)

        {
            var subscription = (await _subscriptionRepository.GetProviderSubscription(ProviderId)).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                subscription = subscription
            .Where(u => u.user.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        u.provider.food_items.FirstOrDefault().food_name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            }

            if (!string.IsNullOrEmpty(filter))
            {
                subscription = subscription.Where(o => !string.IsNullOrEmpty(o.start_date) && o.start_date.Substring(0, 10) == filter).ToList();
            }
            var totalCount = subscription.Count;
            var firstCategoryId = subscription.Select(p => p.details?.FirstOrDefault()?.category_id ?? Guid.Empty).FirstOrDefault();
            var categoryName = await categoryById(firstCategoryId);
        
            var Allorder = subscription.Select(o => new GetSubscriptionDetailsDto
            {
                user_name = o.user.name,
                address = o.user.address,
                city = o.user.city,
                ph_no = o.user.phone,
                fooditem_name = o.provider.food_items?.FirstOrDefault().food_name,
                menu_name = o.provider.menus?.FirstOrDefault().name,
                category_name=categoryName, 
                total_price = o.total_price,
                start_date = o.start_date,
                is_active = o.is_active

            }).ToList();
            var pagedOrders = Allorder.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new AllSubByProviderDto
            {
                TotalCount = totalCount,
                Allsubscription = pagedOrders
            };
            return new List<AllSubByProviderDto> { result };
        }
        public async Task<string> categoryById(Guid id)
        {
          return  await _subscriptionRepository.categoryById(id);
        }
    }

}
