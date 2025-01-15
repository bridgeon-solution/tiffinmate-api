using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Entities
{
    public class Rating:AuditableEntity
    {
        public Guid id { get; set; }
        public Guid provider_id { get; set; }
        public Guid user_id { get; set; }
        public int rating { get; set; }
        public Provider provider { get; set; }
        public User user { get; set; }
    }
}
