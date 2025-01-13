using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.NotificationInterface
{
    public interface INotificationService
    {
        Task NotifyAdminsAsync(string recipient_type, string title, string message, string notification_type, string? recipient_id = null);
    }
}
