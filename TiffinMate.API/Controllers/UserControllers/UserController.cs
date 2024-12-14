using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.AuthInterface;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.DAL.Entities;

namespace TiffinMate.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IOtpService _otpService;
        public UserController(IAuthService authService, IOtpService otpService,IUserService userService)
        {
            _authService = authService;
            _otpService = otpService;
            _userService = userService;

        }
        [HttpPost("signup")]
        public async Task<IActionResult> Register(RegisterUserDto userDto)
        {

            try
            {
                var response = await _authService.RegisterUser(userDto);
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

            var res = await _authService.VerifyUserOtp(verifyOtpDto.Phone, verifyOtpDto.Otp);
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
                var response = await _authService.LoginUser(userDto);
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
        [HttpGet("all_users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(new ApiResponse<List<User>>("success", "users getted succesfuly", result, HttpStatusCode.OK, ""));


            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
        [HttpPut("block_unblock")]
        public async Task<IActionResult> BlockUnblockUser(Guid id)
        {
            try
            {
                var response = await _userService.BlockUnblock(id);
                return Ok(new ApiResponse<BlockUnblockResponse>("success", "user blocked/unblocked succesfuly", response, HttpStatusCode.OK, ""));


            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
              

        }
    }
}
