using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class OrderDetailsResponseDTO
    {
        public string date { get; set; }

        public Guid menu_id { get; set; }
        public string provider { get; set; }
        public Guid user_id { get; set; }

        public decimal total_price { get; set; }

        public List<OrderDetailsDto> details { get; set; }
    }
}
