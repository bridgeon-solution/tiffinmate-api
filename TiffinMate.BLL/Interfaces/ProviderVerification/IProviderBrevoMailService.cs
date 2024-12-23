using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.ProviderVerification
{
    public interface IProviderBrevoMailService
    {
        Task<bool> SendOtpEmailAsync(string to, string otp);
        Task<bool> Rejectmail(string to);
        Task<bool> SendOtpForgetPassword(string to, string otp);
    }
}
