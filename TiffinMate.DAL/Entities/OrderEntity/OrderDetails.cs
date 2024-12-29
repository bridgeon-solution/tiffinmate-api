    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace TiffinMate.DAL.Entities.OrderEntity
    {
        public class OrderDetails :AuditableEntity
        {
            public Guid id { get; set; }
            public Guid order_id { get; set; }
            public string user_name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string fooditem_name { get; set; }
            public string fooditem_image { get; set; }
            public Guid category_id { get; set; }


            public Order order { get; set; }
        
        }
    }
