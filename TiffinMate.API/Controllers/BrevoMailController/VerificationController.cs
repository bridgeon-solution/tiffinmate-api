using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.Interfaces.ProviderVerification;

namespace TiffinMate.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly IProviderVerificationService _verificationService;

        public VerificationController(IProviderVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [HttpPost("send-password/{providerId}")]
        public async Task<IActionResult> SendOtpEmail(Guid providerId)
        {
            if (providerId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<string>(
                    "failure",
                    "Invalid Input",
                    null,
                    HttpStatusCode.BadRequest,
                    "Provider ID is required."));
            }
            try
            {
                var success = await _verificationService.SendPassword(providerId);

                if (success)
                {
                    return Ok(new ApiResponse<string>(
                        "success",
                        "OTP Sent",
                        "OTP sent and password updated successfully.",
                        HttpStatusCode.OK,
                        ""));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>(
                        "failure",
                        "Operation Failed",
                        null,
                        HttpStatusCode.InternalServerError,
                        "Failed to send OTP or update password."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>(
                    "failure",
                    "Error Occurred",
                    null,
                    HttpStatusCode.InternalServerError,
                    ex.Message));
            }
        }

        [HttpPost("removeprovider")]
        public async Task<IActionResult> RemoveProvider(Guid providerId)
        {
            if (providerId == Guid.Empty)
            {
                return BadRequest(new ApiResponse<string>(
                    "failure",
                    "Invalid Input",
                    null,
                    HttpStatusCode.BadRequest,
                    "Provider ID is required."));
            }

            try
            {
                var success = await _verificationService.RemoveData(providerId);

                if (success)
                {
                    return Ok(new ApiResponse<string>(
                        "success",
                        "Provider Removed",
                        "Removed Provider successfully.",
                        HttpStatusCode.OK,
                        ""));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>(
                        "failure",
                        "Operation Failed",
                        null,
                        HttpStatusCode.InternalServerError,
                        "Failed to remove provider."));
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new ApiResponse<string>(
                    "failure",
                    "Error Occurred",
                    null,
                    HttpStatusCode.InternalServerError,
                    ex.Message));
            }
        }
    }
}
