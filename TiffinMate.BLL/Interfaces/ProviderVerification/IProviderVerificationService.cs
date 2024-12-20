using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;

namespace TiffinMate.BLL.Interfaces.ProviderVerification
{
    public interface IProviderVerificationService
    {
        Task<bool> SendPassword(Guid providerId);

        Task<bool> RemoveData(Guid providerId);
        Task<string> SendResetOtp(ForgotPasswordDto forgotPasswordDto);
        bool VerifyEmailOtp(VerifyEmailOtpDto verifyEmailOtp);
        Task<string> ResetPassword(ResetPasswordDto resetPasswordDto);

    }
}
