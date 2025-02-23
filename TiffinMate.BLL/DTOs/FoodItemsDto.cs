using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs
{
    public class FoodItemsDto
    {
        public string food_name { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public string day { get; set; }
        public string image { get;set; }
    }
}
