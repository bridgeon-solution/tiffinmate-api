using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.UserInterfaces
{
    public interface IBrevoMailService
    {
        string GenerateOtp();
        Task<bool> SendOtpEmailAsync(string to, string otp);

    }
}
