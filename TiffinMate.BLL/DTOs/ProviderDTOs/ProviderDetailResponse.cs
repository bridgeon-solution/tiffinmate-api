using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class ProviderDetailResponse
    {
        public Guid provider_id { get; set; }
        public string resturent_name { get; set; }
        public string image { get; set; }
    }
}
