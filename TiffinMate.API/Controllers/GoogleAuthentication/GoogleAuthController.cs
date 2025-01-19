using Asp.Versioning;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Supabase.Gotrue;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Services.UserServices;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;

namespace TiffinMate.API.Controllers.GoogleAuthentication
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IAuthRepository _authrepo;
        public GoogleAuthController(IAuthRepository authrepo)
        {
            _authrepo = authrepo;
        }

        [HttpGet("SignIn")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            var redirectUrl = "http://localhost:5174/google"; 
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl
            };
            return Challenge(properties, "Google");
        }

        [HttpGet("HandleGoogleResponse")]
        public async Task<IActionResult> HandleGoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                var error = result.Failure?.Message;
                return Unauthorized($"Authentication failed: {error}");
            }

            var claims = result.Principal.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
            {
                return BadRequest("Google authentication failed to provide necessary information.");
            }

            var existingUser = await _authrepo.UserExists(email);
            if (!existingUser)
            {
                var id = Guid.NewGuid();
                var newUser = new TiffinMate.DAL.Entities.User
                {
                    id = id,
                    email = email,
                    name = name,
                    created_at = DateTime.UtcNow,
                    password = "12345",
                    phone = "1234567"
                };

                await _authrepo.CreateUser(newUser);

                var tokenHelper = new TokenHelper();
                var newRefreshToken = tokenHelper.GenerateRefreshTokenUser(newUser);
                var tokens = new UserToken();
                var jwtToken = tokens.GenerateJwtToken(newUser);

                return Ok(new
                {
                    id = id,
                    IdToken = jwtToken,
                    RefreshToken = newRefreshToken
                });
            }

            return Unauthorized();
        }


        [HttpGet("SignOut")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");

            var googleLogoutUrl = "https://accounts.google.com/Logout";
            return Redirect(googleLogoutUrl);
        }
        
    }
}
