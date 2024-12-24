using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class FoodItemDto
    {
        public Guid category_id { get; set; }
        public Guid provider_id { get; set; }
        public Guid menu_id { get; set; }
        public string food_name { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public string day { get; set; }
       


        

    }
}
