using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Entities
{
    public class Review:AuditableEntity
    {
        public Guid id { get; set; }
        public Guid ProviderId { get; set; }
        public Guid UserId { get; set; }
        public string review { get; set; }
        public Provider Provider { get; set; }
        public User User { get; set; }
    }
}
