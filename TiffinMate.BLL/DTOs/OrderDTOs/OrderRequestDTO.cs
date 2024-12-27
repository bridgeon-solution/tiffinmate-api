using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class OrderRequestDTO
    {
        public string user_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
    }
}
