﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.UserDTOs
{
    public class LoginResponseDto
    {
        public Guid id {  get; set; }
        public string name { get; set; }
        public  string token { get; set; }
        public string message { get; set; }
    }
}
