using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.NotificationInterface
{
    public interface INotificationService
    {
        Task CreateAndSendNotificationAsync(string title, string message);
    }
}
