using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.OrderEntity
{
    public class OrderDetails :AuditableEntity
    {
        public int order_id { get; set; }
        public string user_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        
    }
}
