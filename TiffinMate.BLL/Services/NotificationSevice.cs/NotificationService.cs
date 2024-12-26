using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces.NotificationInterface;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.NotificationInterfaces;

namespace TiffinMate.BLL.Services.NotificationSevice.cs
{
    public class NotificationService:INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // Method to create a notification, save to DB, and broadcast to WebSocket clients
        public async Task CreateAndSendNotificationAsync(string title, string message)
        {
            // 1. Save the notification to the database
            var notification = new Notification
            {
                Title = title,
                Message = message,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            // 2. Broadcast the notification to all connected WebSocket clients
            await WebSocketManager.BroadcastMessage(title, message);
        }
    
}
}
