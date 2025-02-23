﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;

namespace TiffinMate.BLL.Interfaces.AuthInterface
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(RegisterUserDto userDto);
        Task<bool> VerifyUserOtp(VerifyOtpDto verifyOtpDto);
        Task<LoginResponseDto> LoginUser(LoginUserDto userDto);
        Task<string> SendResetOtp(ForgotPasswordDto forgotPasswordDto);
        bool VerifyEmailOtp(VerifyEmailOtpDto verifyEmailOtp);
        Task<string> ResetPassword(ResetPasswordDto resetPasswordDto);


    }
}
