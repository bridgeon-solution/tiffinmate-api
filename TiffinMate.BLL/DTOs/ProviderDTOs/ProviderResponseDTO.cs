using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class ProviderResponseDTO
    {
        public Guid id { get; set; }
        
        public string? email { get; set; }
        public string? certificate { get; set; }
       

        public string? username { get; set; }
       
        public string verification_status { get; set; } = "pending";
        
    }
}
