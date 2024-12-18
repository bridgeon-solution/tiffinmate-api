using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.AuthInterface;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.BLL.Services.UserService;
using TiffinMate.DAL.Entities;

namespace TiffinMate.API.Controllers.UserControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;
        private readonly IUserService _userService;

        public UserController(IAuthService authService, IOtpService otpService, IUserService userService)
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
            try
            {
                if (string.IsNullOrEmpty(verifyOtpDto.phone) || string.IsNullOrEmpty(verifyOtpDto.otp))

                {
                    return BadRequest(new ApiResponse<string>("failure", "Phone number and OTP are required.", null, HttpStatusCode.BadRequest, "Phone number and OTP are required"));


                }
                var res = await _authService.VerifyUserOtp(verifyOtpDto);


                if (!res)
                {
                    return BadRequest(new ApiResponse<string>("failure", "invalid OTP.", null, HttpStatusCode.BadRequest, "Invalid or expired OTP."));


                }
                return Ok(new ApiResponse<bool>("success", "OTP verification succesful",res , HttpStatusCode.OK, ""));

            }
            catch(Exception ex)
            {

                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
            

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            try
            {
                var response = await _authService.LoginUser(userDto);

                if (response.message == "User Not Found")
                {
                    return NotFound(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.NotFound, "user not found"));
                }

                if (response.message == "Invalid Email")
                {
                    return BadRequest(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.BadRequest, "Email or password is incorrect"));
                }

                var result = new ApiResponse<LoginResponseDto>("success", "Login Successful", response, HttpStatusCode.OK, "");
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
                var res = await _otpService.SendSmsAsync(phone);
                return Ok(new ApiResponse<string>("success", "OTP sended succesful", null, HttpStatusCode.OK, ""));

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }

        }
        [HttpPost("forgot-passowrd")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var response = await _authService.SendResetOtp(forgotPasswordDto);
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
                var response = _authService.VerifyEmailOtp(verifyEmailOtp);
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
                var response = await _authService.ResetPassword(resetPasswordDto);
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
                var response = await _authService.SendResetOtp(forgotPasswordDto);
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
        [HttpGet("id")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null)
                {
                    var notFoundResponse = new ApiResponse<string>("failed", "User not found", "", HttpStatusCode.NotFound, "User not found");
                    return NotFound(notFoundResponse);
                }

                var result = new ApiResponse<UserProfileDto>("success", "fetched Successfully", user, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
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
