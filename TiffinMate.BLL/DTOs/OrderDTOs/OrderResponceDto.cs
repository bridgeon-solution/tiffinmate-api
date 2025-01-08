using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class OrderResponceDto
    {
        public Guid orderdetails_id { get; set; }
        public Guid order_id { get; set; }
        public string user_name { get; set; }
        public string city { get; set; }
        public string phone_number { get; set; }
       
       
    }
}
