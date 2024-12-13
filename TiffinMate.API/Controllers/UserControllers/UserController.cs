using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.AuthInterface;
using TiffinMate.BLL.Services.UserService;

namespace TiffinMate.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _userService;
        private readonly IOtpService _otpService;
        public UserController(IAuthService userService, IOtpService otpService)
        {
            _userService = userService;
            _otpService = otpService;

        }
        [HttpPost("signup")]
        public async Task<IActionResult> Register(RegisterUserDto userDto)
        {

            try
            {
                var response = await _userService.RegisterUser(userDto);
                if (!response)
                {
                    return Conflict(new ApiResponse<string>("failure", "registration failed", null, HttpStatusCode.Conflict, "user already exist"));
                }
               
                var result = new ApiResponse<bool>("success", "registration Successfull", response, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
            if (string.IsNullOrEmpty(verifyOtpDto.phone) || string.IsNullOrEmpty(verifyOtpDto.otp))
            {
                return BadRequest(new ApiResponse<string>("failure", "Phone number and OTP are required.", null, HttpStatusCode.BadRequest, "Phone number and OTP are required"));

            }

            var res = await _userService.VerifyUserOtp(verifyOtpDto);
            if (!res)
            {
                return BadRequest(new ApiResponse<string>("failure", "invalid OTP.", null, HttpStatusCode.BadRequest, "Invalid or expired OTP."));


            }
            return Ok(new ApiResponse<string>("success", "OTP verification succesful", null, HttpStatusCode.OK, ""));

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            try
            {
                var response = await _userService.LoginUser(userDto);
                if (response.message == "User Not Found")
                {
                    return NotFound(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.NotFound, "user not found"));
                }
                if (response.message =="Invalid Password")
                {

                    return BadRequest(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.BadRequest, "Email or password is incorrect"));
                }

                var result = new ApiResponse<LoginResponseDto>("success", "Login Successfull", response, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }


        }
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp(string phone)
        {
            try
            {
               var res= await _otpService.SendSmsAsync(phone);
                return Ok(new ApiResponse<string>("success", "OTP sended succesful", null, HttpStatusCode.OK, ""));

            }
            catch(Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
           
        }
        [HttpPost("forgot-passowrd")]
        public async Task<IActionResult>ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var response = await _userService.SendResetOtp(forgotPasswordDto);
                if (response == "Not Found")
                {
                    return NotFound(new ApiResponse<string>("failure", "Failed", null, HttpStatusCode.NotFound, "user not found"));
                }
                if (response == "failed")
                {

                    return BadRequest(new ApiResponse<string>("failure", "Failed", null, HttpStatusCode.BadRequest, "failed"));
                }

                var result = new ApiResponse<string>("success", "otp sended Successfully", response, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
            
        }
        [HttpPost("verify-email-otp")]
        public async Task<IActionResult> VerifyEmailOtp(VerifyEmailOtpDto verifyEmailOtp)
        {
            try
            {
                var response = _userService.VerifyEmailOtp(verifyEmailOtp);
                if (!response)
                {
                    return BadRequest(new ApiResponse<string>("failure", "verification failed", null, HttpStatusCode.BadRequest, "otp verification failed"));
                }

                var result = new ApiResponse<bool>("success", "verification Successfull", response, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }

            
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var response = await _userService.ResetPassword(resetPasswordDto);
                if (response == "User Not Found")
                {
                    return NotFound(new ApiResponse<string>("failure", "Failed", null, HttpStatusCode.NotFound, "user not found"));
                }
                if (response == "updation failed")
                {

                    return BadRequest(new ApiResponse<string>("failure", "Failed", null, HttpStatusCode.BadRequest, "failed"));
                }

                var result = new ApiResponse<string>("success", "password updated Successfully", response, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
            
        }
        [HttpPost("resend-mail-otp")]
        public async Task<IActionResult> ResendMailOtp(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var response = await _userService.SendResetOtp(forgotPasswordDto);
                if (response == "Not Found")
                {
                    return NotFound(new ApiResponse<string>("failure", "Failed", null, HttpStatusCode.NotFound, "user not found"));
                }
                if (response == "failed")
                {

                    return BadRequest(new ApiResponse<string>("failure", "Failed", null, HttpStatusCode.BadRequest, "failed"));
                }

                var result = new ApiResponse<string>("success", "otp sended Successfully", response, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }

        }

    }
}
