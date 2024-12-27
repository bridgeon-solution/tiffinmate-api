using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class Menu :AuditableEntity
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public Guid provider_id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string image { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public bool is_available { get; set; }
        [Required]
        public decimal monthly_plan_amount { get; set; }
        public List<FoodItem> food_items { get; set; }
        public Provider provider { get; set; }
    }
}
