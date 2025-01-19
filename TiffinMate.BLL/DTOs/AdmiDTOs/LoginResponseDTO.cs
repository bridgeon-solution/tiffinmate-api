using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.AdmiDTOs
{
   public class LoginResponseDTO
    {
       public Guid id { get; set; }
        public string name { get; set; }
        
        public string token { get; set; }
        public string message { get; set; }
        public string refresh_token { get; set; }
    }
}
