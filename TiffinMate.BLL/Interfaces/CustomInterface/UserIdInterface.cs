using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.CustomInterface
{
    public interface IUserIdInterface
    {
        public string GetUserId(HubConnectionContext connection);
    }
}
