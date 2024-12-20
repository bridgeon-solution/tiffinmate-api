using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class ProviderDetails:AuditableEntity
    {
        public Guid id { get; set; }
        public Guid provider_id { get; set; }
        public string resturent_name { get; set; }
        public string address { get; set; }
        public int phone_no { get; set; }
        public string location { get; set; }
        public string logo { get; set; }
        public string about { get; set; }
        public string image { get; set; }
        public int account_no { get; set; }
        //public DateTime? created_at { get; set; } = DateTime.UtcNow;
        //public DateTime? updated_at { get; set; } = DateTime.UtcNow;
        public Provider provider { get; set; }
    }
}
