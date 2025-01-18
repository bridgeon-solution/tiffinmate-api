using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Custom
{
    public class CustomUserIdProvider :IUserIdProvider
    { 
        
        public string GetUserId(HubConnectionContext connection)
        {
            var providerid = connection.User?.FindFirst("providerId")?.Value;
            if (string.IsNullOrEmpty(providerid)) {
              providerid=connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;  
            
            }
            return providerid;
        }
    }
}
