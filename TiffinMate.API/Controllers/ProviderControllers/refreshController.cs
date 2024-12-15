using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.ProviderServiceInterafce;

namespace TiffinMate.API.Controllers.ProviderControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class refreshController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public refreshController(IProviderService providerService)
        {

            _providerService = providerService;

        }
        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] refreshTokenDto request)
        {
            if (string.IsNullOrEmpty(request.refresh_token))
            {
                return BadRequest("Refresh token is required.");
            }

            try
            {
                var result = await _providerService.GetRefreshToken(request.refresh_token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
