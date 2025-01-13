using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class OrderDetailsDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string FoodItemName { get; set; }
        public string FoodItemImage { get; set; }
        public string Category { get; set; }
        public string Menu { get; set; }
        public string ph_no { get; set; }

    }
}
