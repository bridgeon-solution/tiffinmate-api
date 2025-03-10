﻿using Asp.Versioning;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using Supabase.Storage;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
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
                    return Ok(new ApiResponse<string>("failure", "registration failed", null, HttpStatusCode.Conflict, "Email already exist"));
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
                    return Ok(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.NotFound, "Invalid credentials. Please try again."));
                }

                if (response.message == "Invalid Email")
                {
                    return Ok(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.BadRequest, "Oops! The email or password you entered is incorrect. Please try again"));
                }
                if (response.message == "User Blocked")
                {
                    return Ok(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.BadRequest, "Your account has been blocked. Please contact support"));
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

        
        [HttpPatch("block")]
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

        [HttpGet]

        public async Task<IActionResult> GetUsers(int pageSize, int page, string search = "", string filter = "")
        {
            try
            {
                var response = await _userService.GetUsers(page, pageSize, search, filter);
                return Ok(new ApiResponse<UserResultDTO>("success", "provider getted", response, HttpStatusCode.OK, ""));


            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
        [HttpGet("{providerId}")]
        public async Task<IActionResult> AllUsers(Guid providerId, int page = 1, int pageSize = 10, string search = null)
        {
            try
            {
                var res = await _userService.UsersLists(providerId, page, pageSize, search);
                var result = new TiffinMate.API.ApiRespons.ApiResponse<List<AllUserOutputDto>>("succesfull", "Getting Users succesfully", res, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                var response = new TiffinMate.API.ApiRespons.ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }

    }
}
