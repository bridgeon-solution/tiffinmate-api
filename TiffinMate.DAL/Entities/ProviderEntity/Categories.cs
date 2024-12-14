using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class Categories
    {
        public DateTime? created_at { get; set; } = DateTime.UtcNow;
        public DateTime? updated_at { get; set; } = DateTime.UtcNow;
        public Guid id { get; set; }
        public string categoryname { get; set; }

        public List<FoodItem> foodItems { get; set; }
    }
}
