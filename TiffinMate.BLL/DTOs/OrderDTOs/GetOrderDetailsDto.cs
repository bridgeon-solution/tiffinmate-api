using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class GetOrderDetailsDto
    {
        public string user_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string ph_no { get; set; }
        public string fooditem_name { get; set; }
        public string category_name { get; set; }
        public string menu_name { get; set; }
        public decimal total_price { get; set; }
        public string? start_date { get; set; }
        public string? fooditem_image { get; set; }
        public OrderStatus order_status { get; set; } = OrderStatus.Pending;

    }
}
