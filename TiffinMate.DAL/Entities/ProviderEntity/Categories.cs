using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class Categories : AuditableEntity
    {
        
        public Guid id { get; set; }
        [Required]
        public string categoryname { get; set; }

        public List<FoodItem> foodItems { get; set; }
    }
}
