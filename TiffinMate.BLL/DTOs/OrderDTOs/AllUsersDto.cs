using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class AllUsersDto
    {
        public string user_name { get; set; }
        public Guid user_id { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string ph_no { get; set; }
        public string email { get; set; }
        public string? image { get; set; }
        public decimal total_price { get; set; }
        public Guid order_id { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime start_date { get; set; }


    }
}
