﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.NotificationInterface;
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
        private readonly INotificationService _notificationService;
        public SubscriptionService(ISubscriptionRepository subscription, AppDbContext appDbContext, IMapper mapper,INotificationService notificationService)
        {
            _subscriptionRepository = subscription;
            _context = appDbContext;
            _mapper = mapper;
            _notificationService = notificationService;

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
            var paymentHistoryId = Guid.NewGuid();
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
            var newPaymentHistory=new PaymentHistory
            {
                id = paymentHistoryId,     
                subscription_id = orderId,
                amount = orderRequestDTO.total_price,
                payment_date = DateTime.UtcNow,
                user_id = orderRequestDTO.user_id,
                order_type="subscription"
                
            };
            await _context.paymentHistory.AddAsync(newPaymentHistory);
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
                    order.order_status = OrderStatus.Confirmed;
                    order.is_active = true;
                    order.order_string = orderDetailsRequestDto.order_string;
                    order.transaction_id = orderDetailsRequestDto.transaction_string;
                    _context.subscriptions.Update(order);
                    await _context.SaveChangesAsync();

                    await _notificationService.NotifyProviderAsync(orderDetailsRequestDto.provider_id.ToString(),
                                                           "New subscription",
                                                           $"You have a new subscription from {orderDetailsRequestDto.user_name}.","Subscription");
                }
                var paymentHistory = await _context.paymentHistory.FirstOrDefaultAsync(p => p.subscription_id == orderId);
                if(paymentHistory != null)
                {
                    paymentHistory.is_paid = true;
                    _context.paymentHistory.Update(paymentHistory);
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


            var Allorder = subscription.Select(o => new GetSubscriptionDetailsDto
            {
                user_name = o.user?.name,
                address = o.user?.address,
                city = o.user?.city,
                ph_no = o.user?.phone,
                category = o.provider.food_items
         .GroupBy(ph => ph.category?.category_name) 
         .Select(group => new CategoryWithFoodItemsDto
         {
             category_name = group.Key, 
             food_Items = group.Select(ph => new FoodItemsDto
             {
                 food_name = ph.food_name,
                 price = ph.price,
                 description = ph.description,
                 day = ph.day,
                 image = ph.image
             }).ToList()
         }).ToList(),
                menu_name = o.provider.menus?.FirstOrDefault()?.name,
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

       
        public async Task<List<AllSubByProviderDto>> SubscriptionLists(Guid ProviderId, int page, int pageSize, string search = null, string filter = null, string toggle = null)

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
            if (!string.IsNullOrEmpty(toggle))
            {
                if (toggle.ToLower() == "true")
                {
                    subscription = subscription.Where(u => u.is_active == true).ToList();
                }
                else if (toggle.ToLower() == "false")
                {
                    subscription = subscription.Where(u => u.is_active == false).ToList();
                }

            }
            var totalCount = subscription.Count;


            var Allorder = subscription.Select(o => new GetSubscriptionDetailsDto
            {
                user_name = o.user?.name,
                address = o.user?.address,
                city = o.user?.city,
                ph_no = o.user?.phone,
                category = o.provider.food_items
        .GroupBy(ph => ph.category?.category_name) 
        .Select(group => new CategoryWithFoodItemsDto
        {
            category_name = group.Key, 
            food_Items = group.Select(ph => new FoodItemsDto
            {
                food_name = ph.food_name,
                price = ph.price,
                description = ph.description,
                day = ph.day,
                image = ph.image
            }).ToList()
        }).ToList(),
                menu_name = o.provider.menus?.FirstOrDefault()?.name,
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
       

        //All_Subsribtion_Orders

        public async Task<AllSubByProviderDto> GetSubscribtionOrders(int page, int pageSize, string search = null, string filter = null)
        {

            var subscription = (await _subscriptionRepository.GetAllSubscription()).ToList();
            var result = subscription.Select(o => new GetSubscriptionDetailsDto
            {
                user_name = o.user?.name,
                provider_name=o.provider.user_name,
                address = o.user?.address,
                city = o.user?.city,
                ph_no = o.user?.phone,
                category = o.provider.food_items
        .GroupBy(ph => ph.category?.category_name)
        .Select(group => new CategoryWithFoodItemsDto
        {
            category_name = group.Key,
            food_Items = group.Select(ph => new FoodItemsDto
            {
                food_name = ph.food_name,
                price = ph.price,
                description = ph.description,
                day = ph.day,
                image = ph.image
            }).ToList()
        }).ToList(),
                menu_name = o.provider.menus?.FirstOrDefault()?.name,
                total_price = o.total_price,
                start_date = o.start_date,
                is_active = o.is_active

            }).ToList();


            if (!string.IsNullOrEmpty(search))
            {
                subscription = subscription
            .Where(u => u.user.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        u.provider.food_items.FirstOrDefault().food_name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

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


            var newResult = new AllSubByProviderDto
            {
                TotalCount = total,
                Allsubscription = pagedUsers
            };

            return newResult;
        }
        public async Task<List<PaymentHistory>> GetPaymentHistory(Guid? id)
        {
            return await _subscriptionRepository.GetPaymentHistory(id);
        }
        public async Task<bool> HandleSubscription(PaymentHistoryRequestDto dto)
        {
            var paymentHistory = await _subscriptionRepository.GetPaymentHistory(dto.payment_id);
            if (paymentHistory == null)
            {
                return false;
            }

            if (dto.action == "renew")
            {
                foreach (var payment in paymentHistory)
                {
                    payment.is_paid = true;
                    payment.payment_date = DateTime.UtcNow;
                    payment.updated_at = DateTime.UtcNow;
                    await _subscriptionRepository.UpdatePaymentHistoryAsync(payment);

                }
                return true;
            }
            if (dto.action == "cancel")
            {
                var subscription = await _subscriptionRepository.GetSubscriptionByid(paymentHistory.First().subscription_id);
                if (subscription != null)
                {
                    subscription.is_active = false;
                    subscription.cancelled_at = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                    subscription.updated_at = DateTime.UtcNow;
                    await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
                    return true;
                }
            }
            return false;
        }
        public async Task<List<SubscriptionDetailsDto>> GetSubscriptionByUser(Guid userId)
        {
            var result = await _context.subscriptions
                .Include(s => s.provider)
                .Include(s => s.menu)
                    .ThenInclude(m => m.food_items) 
                .Include(s => s.details)
                    .ThenInclude(sd => sd.Category)
                .Where(s => s.user_id == userId) 
                .Select(s => new SubscriptionDetailsDto
                {
                    user_id = s.user_id,
                    provider = s.provider.user_name,
                    total_amount = s.menu.monthly_plan_amount,
                    subscription = s.details.GroupBy(sd => sd.Category.category_name)
                        .Select(c => new SubscriptionDto
                        {
                            category = c.Key,
                            fooditems = c.SelectMany(cg => cg.subscription.menu.food_items)
                                .Select(f => new FoodItemDto
                                {
                                    day = f.day,
                                    food_name = f.food_name,
                                })
                                .Distinct()
                                .ToList()
                        }).ToList()
                }).ToListAsync();

            return result;
        }

    }




}
