using System;
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
        Task<bool> VerifyUserOtp(string mobileNumber, string otp);
        Task<string> LoginUser(LoginUserDto userDto);


    }
}
