using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.UserDTOs
{
    public class VerifyOtpDto
    {
        public string Phone { get; set; }
        public string Otp { get; set; }
    }
}
