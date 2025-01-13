using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class OrderRequestDTO
    {
        
        public string date { get; set; }
       
        public Guid menu_id { get; set; }
        public Guid provider_id { get; set; }
        public Guid user_id { get; set; }

        public decimal total_price { get;set; }
        
        
       
    }
}
