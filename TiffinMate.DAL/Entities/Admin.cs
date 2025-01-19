using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities
{
    public class Admin:AuditableEntity
    {
        public Guid id { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        [EmailAddress]
        public string password { get; set; }
        
        public string user_name { get; set; }
        public string role { get; set; }

        public DateTime created_at { get; set; }
        public string refresh_token { get; set; } = string.Empty;
        public DateTime refreshtoken_expiryDate { get; set; }

    }
}
