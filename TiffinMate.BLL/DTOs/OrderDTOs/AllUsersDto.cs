using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class AllUsersDto
    {
        public string user_name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string ph_no { get; set; }
        public string email { get; set; }
        public string? image { get; set; }

    }
}
