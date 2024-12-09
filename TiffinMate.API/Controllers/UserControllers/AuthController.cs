using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.AuthInterface;

namespace TiffinMate.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userService;
        private readonly IOtpService _otpService;
        public AuthController(IAuthService userService, IOtpService otpService)
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
               
                var result = new ApiResponse<bool>("succes", "registration Successfull", response, HttpStatusCode.OK, "");
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
            if (string.IsNullOrEmpty(verifyOtpDto.Phone) || string.IsNullOrEmpty(verifyOtpDto.Otp))
            {
                return NotFound(new ApiResponse<string>("failure", "Phone number and OTP are required.", null, HttpStatusCode.BadRequest, "Phone number and OTP are required"));

            }

            var res = await _userService.VerifyUserOtp(verifyOtpDto.Phone, verifyOtpDto.Otp);
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
                if (response == "Not Found")
                {
                    return NotFound(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.NotFound, "user not found"));
                }
                if (response=="Invalid Password")
                {

                    return BadRequest(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.BadRequest, "Email or password is incorrect"));
                }

                var result = new ApiResponse<string>("success", "Login Successfull", response, HttpStatusCode.OK, "");
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
    }
}
