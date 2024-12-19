﻿using Amazon.S3;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sprache;
using Supabase.Gotrue;
using System.Collections.Generic;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.BLL.Interfaces.ProviderVerification;
using TiffinMate.BLL.Services.ProviderServices;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using Twilio.Annotations;
using static Supabase.Gotrue.Constants;

namespace TiffinMate.API.Controllers.ControllerProvider
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;
        private readonly IReviewService _reviewService;
        private readonly IProviderVerificationService _verificationService;

        public ProviderController(IProviderService providerService, IReviewService reviewService, IProviderVerificationService verificationService)
        {

            _providerService = providerService;
            _reviewService = reviewService;
            _verificationService=verificationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] ProviderDTO providerDTO, IFormFile certificateFile)
        {

            try
            {
                var response = await _providerService.AddProvider(providerDTO, certificateFile);
                if (!response)
                {
                    return BadRequest(new ApiResponse<string>("failure", "registration failed", null, HttpStatusCode.BadRequest, "certificate is not uploaded"));
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

        [HttpPost("login")]
        public async Task<IActionResult> LoginProvider([FromBody] ProviderLoginDTO providerdto)
        {
            var startTime = DateTime.UtcNow;
            object responseBody = null;

            try
            {

                var response = await _providerService.AddLogin(providerdto);


                responseBody = new ApiResponse<ProviderLoginResponse>("success", "Login successful", response, HttpStatusCode.OK, null);

                return Ok(responseBody);
            }
            catch (Exception ex)
            {
                responseBody = new ApiResponse<string>("failure", "Login failed", ex.Message, HttpStatusCode.InternalServerError, "An error occurred while processing the request.");

                return StatusCode((int)HttpStatusCode.InternalServerError, responseBody);
            }
        }

        [HttpPost("providerdetails")]
        public async Task<IActionResult> ProviderDetails([FromForm] ProviderDetailsDTO providerDetailsDTO, IFormFile logo, IFormFile image)
        {


            try
            {
                var response = await _providerService.AddProviderDetails(providerDetailsDTO, logo, image);
                if (!response)
                {
                    return BadRequest(new ApiResponse<string>("failure", " failed", null, HttpStatusCode.BadRequest, "logo or image is not uploaded"));
                }

                var result = new ApiResponse<bool>("success", " Successfull", response, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }

        [HttpGet]
        public async Task<IActionResult> AllProviders()
        {
            try
            {
                var response = await _providerService.GetProviders();
                return Ok(new ApiResponse<List<ProviderResponseDTO>>("success", "providers getted succesfuly", response, HttpStatusCode.OK, ""));
            }
            catch (Exception ex) {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }


        }

        [HttpPatch("block")]
        public async Task<IActionResult> BlockUnblockUser(Guid id)
        {
            try
            {
                var response = await _providerService.BlockUnblock(id);
                return Ok(new ApiResponse<BlockUnblockResponse>("success", "Vendor blocked/unblocked succesfuly", response, HttpStatusCode.OK, ""));

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }

        }

        [HttpGet("{providerid}/reviews")]
        public async Task<IActionResult> AllReviews (Guid providerid)
        {
            try
            {
                var response = await _reviewService.GetAllProviderReview(providerid);
                return Ok(new ApiResponse<List< AllReview >> ("success", "providers getted succesfuly", response, HttpStatusCode.OK, ""));
            }
            catch(Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
        [HttpPost("review")]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                return BadRequest(new ApiResponse<string>("failure","Invalid Input",null,HttpStatusCode.BadRequest,"Review data is required."));
            }

            try
            {
                var result = await _reviewService.Reviews(reviewDto);

                if (result)
                {
                    return Ok(new ApiResponse<string>("success","Review Added","Review added successfully.",HttpStatusCode.OK,""));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure","Operation Failed",null,HttpStatusCode.InternalServerError,"Failed to add review."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>( "failure", "Error Occurred", null, HttpStatusCode.InternalServerError,ex.Message));
            }
        }
        [HttpGet("{userId}/review")]
        public async Task<IActionResult> GetReviewsByUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<string>("failure", "Invalid Input", null, HttpStatusCode.BadRequest, "User ID is required."));
            }

            try
            {
                var reviews = await _reviewService.GetAllReview(userId);

                if (reviews == null || reviews.Count == 0)
                {
                    return NotFound(new ApiResponse<List<AllReview>>("failure", "No Reviews Found", new List<AllReview>(), HttpStatusCode.NotFound, "No reviews found for the given user."));
                }

                return Ok(new ApiResponse<List<AllReview>>("success", "Reviews Retrieved", reviews, HttpStatusCode.OK, ""));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<List<AllReview>>("failure", "Error Occurred", null, HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpGet("{providerid}")]
        public async Task<IActionResult> GetProviderById(Guid providerid)
        {
            if (providerid == Guid.Empty)
            {
                return BadRequest(new ApiResponse<string>("failure", "Invalid Input", null, HttpStatusCode.BadRequest, "User ID is required."));
            }

            var provider = await _providerService.ProviderById(providerid);
            if (provider == null)
            {
                return NotFound(new ApiResponse<ProviderByIdDto>("failure", "No Provider Found", null, HttpStatusCode.NotFound, "No provider found for the given ID."));
            }

            return Ok(new ApiResponse<ProviderByIdDto>("success", "Provider Retrieved", provider, HttpStatusCode.OK, ""));
        }
        [HttpPost("forgot-passowrd")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                var response = await _verificationService.SendResetOtp(forgotPasswordDto);
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
                var response = _verificationService.VerifyEmailOtp(verifyEmailOtp);
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
                var response = await _verificationService.ResetPassword(resetPasswordDto);
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
                var response = await _verificationService.SendResetOtp(forgotPasswordDto);
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

