using Amazon.S3;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sprache;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.BLL.Services.ProviderServices;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;

namespace TiffinMate.API.Controllers.ControllerProvider
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {

            _providerService = providerService;

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

        [HttpGet("all_providers")]
        public async Task<IActionResult> AllProviders()
        {
            try
            {
                var response = await _providerService.GetProviders();
                return Ok(new ApiResponse<List<Provider>>("success", "providers getted succesfuly", response, HttpStatusCode.OK, ""));
            }
            catch (Exception ex) {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
            

        }

    }
}
