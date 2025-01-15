using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.DAL.Entities
{
    public class User:AuditableEntity
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        [Phone]
        public string phone { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string role { get; set; }
        public bool subscription_status { get; set; }
        public string? address { get; set; }
        public string? city { get; set; }
        public string? image { get; set; }
        public bool is_blocked { get; set; }
        public string refresh_token { get; set; } = string.Empty;
        public DateTime refreshtoken_expiryDate { get; set; }
        public ICollection<Review> review { get; set; }
        public ICollection<Order> order { get; set; }
        public ICollection<Subscription> subscription { get; set; }
        public ICollection<Rating> rating { get; set; }

    }
}
