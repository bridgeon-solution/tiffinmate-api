using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class orderDataDto
    {
        public Guid order_id { get; set; }
        public Guid menu_id { get; set; }
        public string start_date { get; set; }
        public decimal total_price { get; set; }
        public string transaction_id { get; set; }
        public string order_string { get; set; }
    }
}
