using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.AdmiDTO;
using TiffinMate.BLL.Interfaces.AdminInterface;
using Twilio.Rest.Trunking.V1;

namespace TiffinMate.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginDTO loginDTO)
        {
            try
            {
                var response = await _adminService.AdminLogin(loginDTO);
                if (response == "Not Found")
                {
                    return NotFound(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.NotFound, "Admin not found"));
                }
                if (response == "invalid email" || response == "invalid password")
                {

                    return BadRequest(new ApiResponse<string>("failure", "Login Failed", null, HttpStatusCode.BadRequest, "Email or password is incorrect"));
                }

                var result = new ApiResponse<string>("success", "Login Successfull", response, HttpStatusCode.OK, "");
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
