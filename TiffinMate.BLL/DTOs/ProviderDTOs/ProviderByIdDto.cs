using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class ProviderByIdDto
    {
        public string username { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public int phone_no { get; set; }
        public string verification_status { get; set; }
        public string image { get; set; }
        public DateTime? created_at { get; set; }
        
        public string? certificate { get; set; }
    }
}
