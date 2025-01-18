﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.BLL.Interfaces.OrderServiceInterface
{
    public interface ISubscriptionService
    {
        Task<AllOrderDTO> GetSubscribtionOrders(int page, int pageSize, string search = null, string filter = null);
        Task<List<AllSubByProviderDto>> SubscriptionLists(Guid ProviderId, int page, int pageSize, string search = null, string filter = null, string toggle = null);
        Task<List<AllSubByProviderDto>> SubscriptionLists(Guid ProviderId, int page, int pageSize, string search = null, string filter = null);
        Task<OrderRequestDTO> SubscriptionGetedById(Guid OrderId);
        Task<OrderResponceDto> SubscriptionDetailsCreate(OrderDetailsRequestDto orderDetailsRequestDto, Guid orderId);
        Task<Guid> SubscriptionCreate(OrderRequestDTO orderRequestDTO);
    }
}
