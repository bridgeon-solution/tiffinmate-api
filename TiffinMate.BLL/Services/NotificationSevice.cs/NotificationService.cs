using TiffinMate.BLL.Interfaces.NotificationInterface;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.NotificationInterfaces;

namespace TiffinMate.BLL.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task CreateAndSendNotificationAsync(string title, string message)
        {
            var notification = new Notification
            {
                title = title,
                message = message,
                
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
            await WebSocketManager.BroadcastMessage(title, message);
        }
    }
}
