using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.AuthInterface
{
    public interface IOtpService
    {     
        Task<string> SendSmsAsync(string mobileNumber);
        Task<bool> VerifyOtpAsync(string mobileNumber, string otp);
    }
}
