using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.NotificationDTOs;

namespace TiffinMate.BLL.Interfaces.NotificationInterface
{
    public interface INotificationService
    {
        Task NotifyAdminsAsync( string title, string message, string notification_type, string? recipient_id = null);
        Task<List<NotificationAdminResponceDto>> getnotification(string recipientType);
        Task<bool> MarkAsDeleted();
        Task NotifyProviderAsync(string providerId, string title, string message,string notification_type);

        Task NotifyUserAsync(string userId, string title, string message,string notification_type);
        Task NotifySpecificUsersAsync(List<string> userIds, string title, string message, string notification_type);
        Task NotifyAllUsersAsync(string userId, string title, string message, string notification_type);
    }
}
