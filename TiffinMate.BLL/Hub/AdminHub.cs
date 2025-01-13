using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Hubs
{
    public class NotificationHub :Hub
    {
        public async Task SendMessageToAdmin(string recipient_type ,string title,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",recipient_type,title, message);
        }
    }
}
