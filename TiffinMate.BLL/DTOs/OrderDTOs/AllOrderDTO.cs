using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class AllOrderDTO
    {
        public int TotalCount { get; set; }
        public List<OrderDetailsResponseDTO> AllDetails { get; set; }
    }
}
