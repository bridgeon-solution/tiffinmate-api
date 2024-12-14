using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class Provider : AuditableEntity
    {
        public Guid id { get; set; }
        //public DateTime? created_at { get; set; } = DateTime.UtcNow;
        [EmailAddress] 
        public string? email { get; set; }
        public string? certificate { get; set; }
        public string? password { get; set; }
        public string role { get; set; }

        public string? username { get; set; }
        //public DateTime? updated_at { get; set; } = DateTime.UtcNow;
        public bool is_certificate_verified { get; set; } = false;
        public string refresh_token { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryDate { get; set; }
        public ProviderDetails ProviderDetails { get; set; }
    }
}
