using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class ProviderResponseDTO:AuditableEntity
    {
        public Guid id { get; set; }
        
        public string? email { get; set; }
        public string? certificate { get; set; }
       

        public string? user_name { get; set; }
        public bool is_blocked { get; set; }


        public string verification_status { get; set; } = "pending";
        
    }
}
