using Amazon.S3;
using Amazon.S3.Model;
using Asp.Versioning;
using AutoMapper;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sprache;
using Supabase.Gotrue;
using System.Collections.Generic;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs;
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
        private readonly IRatingService _ratingService;
        private readonly IProviderVerificationService _verificationService;

        public ProviderController(IProviderService providerService, IReviewService reviewService, IProviderVerificationService verificationService, IRatingService ratingService)
        {

            _providerService = providerService;
            _reviewService = reviewService;
            _verificationService = verificationService;
            _ratingService = ratingService;
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

        [HttpPost("details")]
        public async Task<IActionResult> ProviderDetails([FromForm] ProviderDetailsDTO providerDetailsDTO, IFormFile logo, IFormFile image)
        {
            try
            {
                var response = await _providerService.AddProviderDetails(providerDetailsDTO, logo, image);
                if (!response)
                {
                    return BadRequest(new ApiResponse<string>("failure", "Upload failed", null, HttpStatusCode.BadRequest, "Logo or image is not uploaded"));
                }

                var result = new ApiResponse<bool>("success", "Successfully added details", response, HttpStatusCode.OK, "");
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Provider details already exist"))
                {
                    return Conflict(new ApiResponse<string>("failure", "Conflict", null, HttpStatusCode.Conflict, ex.Message));
                }

                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "Error occurred");
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
        public async Task<IActionResult> AllReviews(Guid providerid)
        {
            try
            {
                var response = await _reviewService.GetAllProviderReview(providerid);
                return Ok(new ApiResponse<List<AllReview>>("success", "providers getted succesfuly", response, HttpStatusCode.OK, ""));
            }
            catch (Exception ex)
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
                return BadRequest(new ApiResponse<string>("failure", "Invalid Input", null, HttpStatusCode.BadRequest, "Review data is required."));
            }

            try
            {
                var result = await _reviewService.Reviews(reviewDto);

                if (result)
                {
                    return Ok(new ApiResponse<string>("success", "Review Added", "Review added successfully.", HttpStatusCode.OK, ""));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Operation Failed", null, HttpStatusCode.InternalServerError, "Failed to add review."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Error Occurred", null, HttpStatusCode.InternalServerError, ex.Message));
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

        [HttpPost("rating")]
        public async Task<IActionResult> AddRating(RatingRequestDto ratingDto)
        {
            if (ratingDto == null)
            {
                return BadRequest(new ApiResponse<string>("failure", "Invalid Input", null, HttpStatusCode.BadRequest, "Rating data is required."));
            }

            try
            {
                var result = await _ratingService.PostRating(ratingDto);

                if (result)
                {
                    return Ok(new ApiResponse<string>("success", "Rating Added", "Rating added successfully.", HttpStatusCode.OK, ""));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Operation Failed", null, HttpStatusCode.InternalServerError, "Failed to add rating."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Error Occurred", null, HttpStatusCode.InternalServerError, ex.Message));
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
                return Ok(new ApiResponse<ProviderByIdDto>("failure", "No Provider Found", null, HttpStatusCode.NotFound, "No provider found for the given ID."));
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
        [HttpGet("details")]

        public async Task<IActionResult> GetProvidersWithDetail()
        {
            try
            {
                var res = await _providerService.GetProvidersWithDetail();
                return Ok(new ApiResponse<List<ProviderDetailResponse>>("success", "Providers fetched successfully", res, HttpStatusCode.OK, null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Error Occurred", null, HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpGet("{id:Guid}/details")]
        public async Task<IActionResult> GetProviderDetails(Guid id)
        {
            try
            {
                var res = await _providerService.GetProviderDetailsById(id);
                return Ok(new ApiResponse<ProviderDetailedDTO>("success", "Provider fetched successfully", res, HttpStatusCode.OK, null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Error Occurred", null, HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProviders(int page, int pageSize, string search = null, string filter = null, string verifystatus = null)
        {
            try
            {
                var res = await _providerService.GetProviders(page, pageSize, search, filter, verifystatus);
                return Ok(new ApiResponse<ProviderResultDTO>("success", "Provider fetched successfully", res, HttpStatusCode.OK, null));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Error Occurred", null, HttpStatusCode.InternalServerError, ex.Message));
            }

        }

        [HttpPut("editdetails")]
        public async Task<IActionResult> EditProviderDetails([FromForm] EditDetailsDto editProviderDTO, IFormFile logo)
        {
            try
            {
                var result = await _providerService.EditDetails(editProviderDTO, logo);

                // Convert result to string if necessary
                return Ok(new ApiResponse<string>("success", "Provider details edited successfully", result.ToString(), HttpStatusCode.OK, ""));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Failed", null, HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet("{providerId}/reviews/list")]
        public async Task<IActionResult> ReviewsList(
    Guid providerId,
    int page = 1,
    int pageSize = 10,
    string search = null,
    string filter = null)
        {
            try
            {

                var response = await _reviewService.ReviewsList(providerId, page, pageSize, search, filter);


                if (response == null)
                {
                    return NotFound(new ApiResponse<string>("failure", "No reviews found", null, HttpStatusCode.NotFound, "No reviews available for the given provider."));
                }


                var paginationReviewsList = new List<PaginationReview> { response };

                return Ok(new ApiResponse<List<PaginationReview>>("success", "Reviews retrieved successfully", paginationReviewsList, HttpStatusCode.OK, ""));
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>("failure", "Error occurred", ex.Message, HttpStatusCode.InternalServerError, "An error occurred while fetching reviews."));
            }
        }
        [HttpGet("CheckDetail")]
        public async Task<IActionResult> ExistDetail(Guid providerId)
        {

            try
            {
                var response = await _providerService.DetailsExist(providerId);
               
                var result = new ApiResponse<bool>("success", "Added details", response, HttpStatusCode.OK, "");
                return Ok(result);

            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);

            }
        }
        [HttpGet("Payment/{pro_id}")]
        public async Task<IActionResult> GetPaymentDetails(Guid pro_id, int page, int pageSize, string search = null)
        {
            try
            {
                var response = await _providerService.GetTransactionByProviderId(pro_id,page,pageSize,search);

                var result = new ApiResponse<List<AllTransactionByProviderDto>>("success", "Getting successfully", response, HttpStatusCode.OK, "");
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

