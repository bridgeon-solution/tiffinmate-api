using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class OrderDetailsRequestDto
    {
        public string address { get; set; }
        public List<Guid> categories { get; set; }
        public string city { get; set; }
        public string date { get; set; }

        public Guid menu_id { get; set; }

        public string ph_no { get; set; }

        public Guid provider_id { get; set; }
        public string user_name { get; set; }

        public string order_string { get; set; }
        public string transaction_string { get; set; }



    }
}
