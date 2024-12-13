using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.UserDTOs
{
    public class VerifyEmailOtpDto
    {
        public string email { get; set; }
        public string otp { get; set; }
    }
}
