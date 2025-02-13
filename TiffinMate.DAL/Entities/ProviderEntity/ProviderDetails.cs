using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class ProviderDetails:AuditableEntity
    {
        public Guid id { get; set; }
        public Guid provider_id { get; set; }
        [Required]
        public string resturent_name { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$")]
        public string phone_no { get; set; }
        [Required]
        public string location { get; set; }
        [Required]
        public string logo { get; set; }
        [Required]
        public string about { get; set; }
        [Required]
        public string image { get; set; }
        [Required]
        public int account_no { get; set; }
        public Provider Provider { get; set; }
    }
}
