using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class LoginResponse
    {
        public string? Id { get; set; }       
        public string? username { get; set; }
        public string? Token { get; set; }   
        public string? Message { get; set; }  
    }
}
