using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Entities.OrderEntity
{
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Delivered = 2,
        Cancelled = 3,
    }
    public class Order : AuditableEntity
    {
        public Guid id { get; set; }
        public Guid user_id { get; set; }
        public Guid provider_id { get; set; }
        public Guid menu_id { get; set; }
        public string start_date { get; set; }  
        public decimal total_price { get; set; }
        public string? order_string { get; set; }
        public string? transaction_id { get; set; }
        public OrderStatus order_status { get; set; }=OrderStatus.Pending;
        public List<OrderDetails> details { get; set; }
        public User user { get; set; }

        public Provider provider { get; set; }
       

    }
}
