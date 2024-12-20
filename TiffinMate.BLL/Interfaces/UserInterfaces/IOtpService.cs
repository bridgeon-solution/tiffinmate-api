using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;

namespace TiffinMate.BLL.Interfaces.AuthInterface
{
    public interface IOtpService
    {     
        Task<bool> SendSmsAsync(string mobileNumber);
        Task<bool> VerifyOtpAsync(VerifyOtpDto verifyOtpDto);
    }
}
