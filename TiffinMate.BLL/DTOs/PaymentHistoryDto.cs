using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs
{
    public class PaymentHistoryDto
    {
        public Guid id { get; set; }
        public decimal amount { get; set; }
        public string user_name { get; set; }
        public DateTime payment_date { get; set; }
        public bool is_paid { get; set; } = false;
        public string order_type { get; set; }

    }
}
