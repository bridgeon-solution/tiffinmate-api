using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class Categories : AuditableEntity
    {
        
        public Guid id { get; set; }
        [Required]
        public string? category_name { get; set; }

        public List<FoodItem> food_items { get; set; }
        public ICollection<OrderDetails> order_details { get; set; }
        public ICollection<SubscriptionDetails> subscription_details { get; set; }


        
        
    }
}
