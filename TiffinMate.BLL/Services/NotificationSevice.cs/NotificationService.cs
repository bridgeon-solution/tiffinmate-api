using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TiffinMate.BLL.Interfaces.NotificationInterface;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.NotificationInterfaces;
using TiffinMate.BLL.Hubs;
using Org.BouncyCastle.Cms;
using TiffinMate.BLL.DTOs.NotificationDTOs;
using AutoMapper;
using CloudinaryDotNet.Core;

namespace TiffinMate.BLL.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(IHubContext<NotificationHub> hubContext, INotificationRepository notificationRepository,IMapper mapper)
        {
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public async Task NotifyAdminsAsync(string recipient_type, string title, string message, string notification_type, string? recipient_id = null)
        {
            var notification = new Notification
            {
                title = title,
                message = message,
                recipient_type= recipient_type,
                notification_type= notification_type,
                recipient_id= recipient_id,
                isread =false
                
            };
            await _notificationRepository.AddAsync(notification);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", recipient_type,title, message);
        }

        public async Task NotifyProviderAsync(string providerId, string title, string message,string notification_type)
        {
            var notification = new Notification
            {
                title = title,
                message = message,
                recipient_type = "Provider",
                notification_type = notification_type,
                recipient_id = providerId,
                isread = false
            };
            await _notificationRepository.AddAsync(notification);
            await _hubContext.Clients.Group(providerId).SendAsync("RecieveMessage", title, message);
        }

        public async Task NotifyUserAsync(string userId, string title, string message,string notification_type)
        {
            var notification = new Notification
            {
                title = title,
                message = message,
                recipient_type = "User",
                notification_type = notification_type,
                recipient_id = userId,
                isread = false
            };
            await _notificationRepository.AddAsync(notification);
            await _hubContext.Clients.Group(userId).SendAsync("RecieveMessage", title, message);
        }

        public async Task NotifyAllUsersAsync(string userId, string title, string message, string notification_type)
        {
            var notification = new Notification
            {
                title = title,
                message = message,
                recipient_type = "All Users",
                notification_type = notification_type,
                recipient_id = null,
                isread = false
            };
            await _notificationRepository.AddAsync(notification);
            await _hubContext.Clients.All.SendAsync("RecieveMessage","All Users", title, message);
        }

        public async Task NotifySpecificUsersAsync(List<string>userIds ,string title,string message,string notification_type)
        {
            foreach (var userId in userIds)
            {
                var notification = new Notification
                {
                    title = title,
                    message = message,
                    recipient_type = "User",
                    notification_type = notification_type, 
                    recipient_id = userId,
                    isread = false
                };
                await _notificationRepository.AddAsync(notification);
                await _hubContext.Clients.Group(userId).SendAsync("RecieveMessage", title, message);
            }
        }

        public async Task<List<NotificationAdminResponceDto>> getnotification(string recipientType)
        {
            var result = await _notificationRepository.GetAdminNotification(recipientType);
            if (result == null)
            {
                return null;
            }
            return _mapper.Map<List<NotificationAdminResponceDto>>(result); 
        }

        public async Task<bool> MarkAsDeleted()
        {
            await _notificationRepository.MarkAllNotificationsDeleted();
            return true;
        }
    }



}
