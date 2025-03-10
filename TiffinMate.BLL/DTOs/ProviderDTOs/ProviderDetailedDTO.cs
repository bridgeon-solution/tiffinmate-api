﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class ProviderDetailedDTO
    {
        public Guid provider_id { get; set; }
        public string resturent_name { get; set; }
        public string address { get; set; }
        public string phone_no { get; set; }
        public string location { get; set; }
        public string image { get; set; }
        public string about { get; set; }
        public string? certificate { get; set; }
        public string? user_name { get; set; }
        public string? email { get; set; }

    }
}
