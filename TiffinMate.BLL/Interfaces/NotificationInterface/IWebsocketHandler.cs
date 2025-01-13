using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.NotificationInterface
{
    public interface IWebsocketHandler
    {
        Task SendMessageToAdminAsync(string message);
        Task AddConnectionAsync(string connectionId, WebSocket webSocket);


    }
}
