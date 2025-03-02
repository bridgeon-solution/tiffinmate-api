using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class OrderDetailsResponseDTO
    {
        public string date { get; set; }
        public Guid order_id { get; set; }

        public Guid menu_id { get; set; }
        public Guid provider_id { get; set; }
        public string provider { get; set; }
        public Guid user_id { get; set; }
        public string user {  get; set; }
        public string address { get; set; }
        public string ph_no { get; set; }
        public string city { get; set; }

        public decimal total_price { get; set; }
        public OrderStatus order_status { get; set; }
        public string cancelled_at { get; set; }

        public List<OrderDetailsDto> details { get; set; }
    }
}
