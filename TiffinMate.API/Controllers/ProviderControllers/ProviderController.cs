using Amazon.S3;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;
using TiffinMate.BLL.Services.ProviderServices;
using TiffinMate.DAL.DbContexts;
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

        [HttpPost("addprovider")]
        public async Task<IActionResult> AddProduct([FromForm] ProviderDTO providerDTO,IFormFile certificateFile)
        {
            try
            {
              
                if (providerDTO == null || certificateFile == null)
                {
                    return BadRequest("Provider data or certificate file is missing.");
                }

             
                var addedProduct = await _providerService.AddProvider(providerDTO, certificateFile);

                return Ok(addedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the product: " + ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginProvider(ProviderLoginDTO providerdto)
        {
            var pro = await _providerService.AddLogin(providerdto);
            return Ok(pro);
        }
        [HttpPost("providerdetails")]
        public async Task<IActionResult> ProviderDetails([FromForm] ProviderDetailsDTO providerDetailsDTO, IFormFile logo, IFormFile image)
        {
           
            var response = await _providerService.AddProviderDetails(providerDetailsDTO, logo, image);
            return Ok(response);
        }

    }
}
