using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
  public class ProviderLoginResponse
    {
        public Guid id { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }
}
