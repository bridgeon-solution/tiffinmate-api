using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class Provider
    {
        public Guid id { get; set; }
        public DateTime? created_at { get; set; }
       
        public string? email { get; set; }
    
        public string? certificate { get; set; }
        public string? password { get; set; }
        public string? location { get; set; }
        public string? username { get; set; }
        public DateTime? updated_at { get; set; }
        public bool is_certificate_verified { get; set; } = false;
    }
}
