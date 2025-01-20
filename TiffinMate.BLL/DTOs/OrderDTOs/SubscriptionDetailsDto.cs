using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class SubscriptionDetailsDto
    {
        public Guid user_id { get; set; }
        public string provider { get; set; }
        public decimal total_amount { get; set; }
        public List<SubscriptionDto> subscription { get; set; }
    }

    public class SubscriptionDto
    {
        public string category { get; set; }
        public List<FoodItemDto> fooditems { get; set; }
    }

}
