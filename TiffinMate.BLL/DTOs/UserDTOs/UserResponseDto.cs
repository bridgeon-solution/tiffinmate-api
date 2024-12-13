using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.UserDTOs
{
    public class UserResponseDto
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}
