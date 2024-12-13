using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class FoodItem
    {
        public DateTime? created_at { get; set; } = DateTime.UtcNow;
        public DateTime? updated_at { get; set; } = DateTime.UtcNow;
       
        public Guid Id { get; set; }
        public string foodname { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public bool is_veg { get; set; }

        public bool is_available { get; set; }
        public string day { get; set; }
        public string image { get; set; }
        public Categories category { get; set; }
        public Guid categoryid { get; set; }

        public Guid providerid { get; set; }
        public Provider provider { get; set; }

    }
}
