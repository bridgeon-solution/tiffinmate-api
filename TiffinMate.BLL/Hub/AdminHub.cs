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
        public async Task SendMessageToAdmin(string recipient_type, string title, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", recipient_type, title, message);
        }


        //sendMessageToProvider
        public async Task sendMessageToProvider(string ProviderId,string title,string message)
        {
            await Clients.Group(ProviderId).SendAsync("RecieveMessage", title, message);
        }

        //sendMessageToUser
        public async Task sendMessageToUser(string userId,string title,string message)
        {
            await Clients.Group(userId).SendAsync("RecieveMessage", title, message);
        }

        //joinProviderGroup
        public async Task joinProviderGroup(string providerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, providerId);
        }

        //RemoveFromProviderGroup
        public async Task RemoveFromProviderGroup(string providerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, providerId);
        }

        public async Task joinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task RemoveFromUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        //sendToAllUsers
        public async Task sendToAllUsers(string title,string message)
        {
            await Clients.All.SendAsync("RecieveMessage", title, message);
        }

        public async Task sendToSpecificGroup(string userid,string title,string message)
        {
            await Clients.User(userid).SendAsync("RecieveMessage", title, message);
        }

        public async Task sendToProviderGroup(string providerid,string title,string message)
        {
            await Clients.Group(providerid).SendAsync("RecieveMessage", title, message);
        }

    }
}
