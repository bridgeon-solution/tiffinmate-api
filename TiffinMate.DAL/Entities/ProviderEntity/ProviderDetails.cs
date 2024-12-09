using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class ProviderDetails
    {
        public Guid id { get; set; }
        public Guid ProviderId { get; set; }
        public string resturent_name { get; set; }
        public string address { get; set; }
        public int phone_no { get; set; }
        public string location { get; set; }
        public string logo { get; set; }
        public string about { get; set; }
        public string image { get; set; }
        public int account_no { get; set; }

        public Provider Provider { get; set; }
    }
}
