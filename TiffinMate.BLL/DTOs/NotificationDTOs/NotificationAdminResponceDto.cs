using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.NotificationDTOs
{
    public class NotificationAdminResponceDto
    {
        public string title { get; set; }
        public string message { get; set; }
        public bool isread { get; set; }
    }
}
