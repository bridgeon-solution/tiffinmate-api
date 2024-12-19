using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class FoodItem : AuditableEntity
    {
       
        public Guid id { get; set; }
        [Required]
        public string food_name { get; set; }
        [Required]
        public decimal price { get; set; }
        public string description { get; set; }
        [Required]
        public Guid menu_id { get; set; }
        [Required]
        public string day { get; set; }
        [Required]
        public string image { get; set; }

       
        public Categories category { get; set; }
        [Required]
        public Guid category_id { get; set; }

        [Required]
        public Guid provider_id { get; set; }

        public Provider provider { get; set; }
        public Menu menu { get; set; }

    }
}
