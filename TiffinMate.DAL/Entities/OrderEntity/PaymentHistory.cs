using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.OrderEntity
{
    public class PaymentHistory:AuditableEntity
    {
        public Guid id { get; set; }
        public Guid user_id { get; set; }
        public decimal amount { get; set; }
        public DateTime payment_date { get; set; }
        public bool is_paid { get; set; } = false;
        public string order_type { get; set; }
        public Guid? order_id { get; set; }
        public Guid? subscription_id { get; set; }
        public Subscription subscription { get; set; }
        public Order order { get; set; }
        public User user { get; set; }  

    }
}
