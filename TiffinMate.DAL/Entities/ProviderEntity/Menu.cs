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
        public List<FoodItem> foodItems { get; set; }
        public Provider Provider { get; set; }
    }
}
