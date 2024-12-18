using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.AdmiDTO;
using TiffinMate.BLL.DTOs.AdmiDTOs;
using TiffinMate.BLL.Interfaces.AdminInterface;
using Twilio.Rest.Trunking.V1;

namespace TiffinMate.API.Controllers.AdminControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDTO loginDTO)
        {
            try
            {
                var response = await _adminService.AdminLogin(loginDTO);
                if (response.message == "Not Found")
                {
                    return Ok(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.NotFound, "Admin not found"));
                }
                if (response.message == "inccorect password")
                {

                    return Ok(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.BadRequest, "Email or password is incorrect"));
                }

                var result = new ApiResponse<LoginResponseDTO>("success", "Login Successfull", response, HttpStatusCode.OK, "");
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
