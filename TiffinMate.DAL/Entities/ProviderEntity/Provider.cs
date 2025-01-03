using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class Provider : AuditableEntity
    {
        public Guid id { get; set; }
        //public DateTime? created_at { get; set; } = DateTime.UtcNow;
        [EmailAddress] 
        public string? email { get; set; }
        public string? certificate { get; set; }
        public string? password { get; set; }
        public string role { get; set; }
        public bool is_blocked { get; set; }

        public string? user_name { get; set; }
        //public DateTime? updated_at { get; set; } = DateTime.UtcNow;
        public string verification_status { get; set; } = "pending";
        public string refresh_token { get; set; } = string.Empty;
        public DateTime refreshtoken_expiryDate { get; set; }
        public ProviderDetails provider_details { get; set; }
        public ICollection<Review> review { get; set; }
        public ICollection<FoodItem> food_items { get; set; }
        public ICollection<Menu> menus { get; set; }
        public ICollection<Order> order { get; set; }
        public ICollection<Subscription> subscription { get; set; }
    }
}
