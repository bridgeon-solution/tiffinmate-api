﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;

namespace TiffinMate.BLL.DTOs.UserDTOs
{
    public class BlockUnblockResponse
    {
        public bool is_blocked { get; set; }
        public string message { get; set; }
    }
}
