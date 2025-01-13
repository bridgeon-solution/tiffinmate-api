using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TiffinMate.BLL.Interfaces.NotificationInterface;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.NotificationInterfaces;
using TiffinMate.BLL.Hubs;
using Org.BouncyCastle.Cms;

namespace TiffinMate.BLL.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(IHubContext<NotificationHub> hubContext, INotificationRepository notificationRepository)
        {
            _hubContext = hubContext;
            _notificationRepository = notificationRepository;
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

    }



}
