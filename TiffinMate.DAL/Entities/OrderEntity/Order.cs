using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Entities.OrderEntity
{
    public class Order : AuditableEntity
    {
        public Guid id { get; set; }
        public Guid user_id { get; set; }
        public Guid category_id { get; set; }
        public Guid provider_id { get; set; }
        public DateTime start_date { get; set; }  
        public decimal total_price { get; set; }


        public User user { get; set; }
        public ICollection<Categories> category { get; set; }

        public Provider provider { get; set; }
        
    }
}
