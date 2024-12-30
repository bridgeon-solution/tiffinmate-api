using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class OrderResponceDto
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProviderId { get; set; }
        public Guid MenuId { get; set; }
        public string StartDate { get; set; }
       
    }
}
