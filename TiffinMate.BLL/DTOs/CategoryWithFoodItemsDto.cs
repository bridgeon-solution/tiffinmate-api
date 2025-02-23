using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs
{
    public class CategoryWithFoodItemsDto
    {
        public string category_name { get; set; }
        public List<FoodItemsDto> food_Items { get; set; }
    }
}
