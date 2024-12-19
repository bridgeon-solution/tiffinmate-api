﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;

namespace TiffinMate.BLL.DTOs.UserDTOs
{
    public class UserResponseDTO:AuditableEntity
    {
        public Guid id { get; set; }
        
        public string name { get; set; }
        public string email { get; set; }

        public bool subscription_status { get; set; }
   
        public bool is_blocked { get; set; }
    }
}
