using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class GetProvidersDTO
    {
        public Guid id { get; set; }
        public string? email { get; set; }
        public string? certificate { get; set; }

        public string? username { get; set; }

        public bool is_certificate_verified { get; set; } = false;

    }
}
