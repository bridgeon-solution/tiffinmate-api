using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class MenuDto
    {
        public Guid id { get; set; }
       
        public Guid provider_id { get; set; }
        
        public string name { get; set; }

        public string image { get; set; }

        public string description { get; set; }
        public decimal monthly_plan_amount { get; set; }
       
        public bool is_available { get; set; }
    }
}
