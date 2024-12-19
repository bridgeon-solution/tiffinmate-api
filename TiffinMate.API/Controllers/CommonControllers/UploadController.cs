using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sprache;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.UserInterfaces;

namespace TiffinMate.API.Controllers.CommonControllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUserService _userService;
        public UploadController(IUserService userService)
        {
            _userService = userService;          
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadProfilePicture([FromForm]Guid id, IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0)
                {
                    var badReq = new ApiResponse<string>("failed", "Image not found", "", HttpStatusCode.BadRequest, "Image file is required");
                    return BadRequest(badReq);
                }

                var imageUrl = await _userService.UploadImage(image);

                var res = new ApiResponse<string>("success", "Image uploaded successfully", imageUrl, HttpStatusCode.OK, "");
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "Error occurred");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserProfileDto reqDto)
        {
            try
            {
                var result = await _userService.UpdateUser(id, reqDto);
                if (result == null)
                {
                    var notfoundRes = new ApiResponse<string>("failed", "User not found", "", HttpStatusCode.NotFound, "User not found");
                    return NotFound(notfoundRes);
                }

                var res = new ApiResponse<string>("success", "updated", result, HttpStatusCode.OK, "");
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse<string>("failed", "", ex.Message, HttpStatusCode.InternalServerError, "error occured");
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

    }
}
