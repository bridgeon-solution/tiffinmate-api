using crypto;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces;
using TiffinMate.BLL.Services.UserServices;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;

namespace TiffinMate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthGoogleController : ControllerBase
    {

        private readonly IGoogleAuth _authService;
        private readonly ILogger<AuthGoogleController> _logger;
        public AuthGoogleController(IGoogleAuth authService,ILogger<AuthGoogleController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<IActionResult> Google([FromBody] UserView userView)
        {
            if (string.IsNullOrEmpty(userView.TokenId))
                return BadRequest("TokenId cannot be null or empty.");

            try
            {
                // Validate the TokenId with Google's library
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { "YOUR_GOOGLE_CLIENT_ID" }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(userView.TokenId, validationSettings);

                // Authenticate user and generate your own JWT
                var user = await _authService.Authenticate(payload);
                var tokens = new UserToken();
                var token = tokens.GenerateJwtToken(user);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Google authentication");
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
