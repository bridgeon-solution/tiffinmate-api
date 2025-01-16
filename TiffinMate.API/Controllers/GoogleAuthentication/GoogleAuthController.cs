using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TiffinMate.API.Controllers.GoogleAuthentication
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        [HttpGet("SignIn")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            var redirectUrl = "https://tiffinmate.online/"; 
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };
            return Challenge(properties, "Google");
        }

        [HttpGet("HandleGoogleResponse")]
        public IActionResult HandleGoogleResponse()
        {
            var result = HttpContext.AuthenticateAsync("Google").Result;
            if (result.Succeeded)
            {
                var claims = result.Principal.Claims;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                // Redirect the user to the desired URL
                return Redirect("https://tiffinmate.online/"); // Redirect to your main page after successful login
            }
            return Unauthorized();
        }

    }
}
