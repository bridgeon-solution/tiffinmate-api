using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class FoodItemDto
    {
        public Guid categoryid { get; set; }
        public Guid providerid { get; set; }
        public string categoryname { get; set; }
        public string foodname { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public bool is_veg { get; set; }

        public bool is_available { get; set; }
        public string day { get; set; }
        //public string image { get; set; }

    }
}
