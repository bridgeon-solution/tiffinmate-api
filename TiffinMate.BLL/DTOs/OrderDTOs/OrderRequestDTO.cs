﻿using System;
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
        public string date { get; set; }
        public List<Guid> categories { get; set; }
        public Guid provider_id { get; set; }
        public Guid menu_id { get; set; }
        public Guid user_id { get; set; }
    }
}
