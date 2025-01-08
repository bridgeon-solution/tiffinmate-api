using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Entities.OrderEntity
{
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
        public bool payment_status { get; set; }=false;

        public List<OrderDetails> details { get; set; }
        public User user { get; set; }

        public Provider provider { get; set; }
       

    }
}
