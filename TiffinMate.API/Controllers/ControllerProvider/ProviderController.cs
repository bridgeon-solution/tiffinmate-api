using Amazon.S3;
using Amazon.S3.Transfer;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly string _bucketName;
        private readonly ILogger<ProviderController> _logger;
        private readonly AppDbContext _context;
        private readonly IProviderRepository _repo;
        private readonly IMapper _mapper;

        public ProviderController(IProviderService providerService, IConfiguration configuration, ILogger<ProviderController> logger, AppDbContext context,IProviderRepository repo, IMapper mapper)
        {

            _providerService = providerService;
            _logger = logger;
            _bucketName = configuration["AWS:BucketName"] ?? throw new ArgumentNullException("BucketName");
            _context = context;
            _repo = repo;
            _mapper= mapper;
        }

        [HttpPost("UploadCertificate")]

        public async Task<IActionResult> UploadCertificate([FromForm] ProviderDTO providerDto, IFormFile certificate)
        {
            if (certificate == null || certificate.Length == 0)
                return BadRequest("Certificate file is required.");

            if (string.IsNullOrEmpty(providerDto.username) || string.IsNullOrEmpty(providerDto.email))
                return BadRequest("Username and Email are required.");

            try
            {
                // Generate a unique file name
                var uniqueFileName = $"{Guid.NewGuid()}_{certificate.FileName}";

                // Convert IFormFile to Stream and pass to service
                using (var stream = certificate.OpenReadStream())
                {
                    var certificateUrl = await _providerService.UploadCertificateToS3(stream, uniqueFileName, certificate.ContentType);

                    // Simulate saving provider information (save to DB if needed)
                    var provider = new DAL.Entities.ProviderEntity.Provider
                    {
                        
                        created_at = DateTime.UtcNow,
                      
                        username = providerDto.username,
                        email = providerDto.email,
                        
                        is_certificate_verified = false,
                        certificate = certificateUrl,
                      
                        
                        updated_at = DateTime.UtcNow
                       
               
                        
                    };
                    _repo.AddProviderAsync(provider);
                    


                    // Return response
                    return Ok(new { message = "Certificate uploaded successfully!", provider });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading the certificate.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginProvider(ProviderLoginDTO loginProvider)
        {
            try
            {
                var response = await _providerService.LoginProvider(loginProvider);
                if (response.Token == null)
                    return Unauthorized(new { response.Message });

                return Ok(response);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("GenerateAndSendPassword")]
        public async Task<IActionResult>GenerateAndSendPassword(Guid id)
        {
            try
            {
                var result = await _providerService.GenereateAndSendPassword(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
