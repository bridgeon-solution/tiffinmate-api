using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TiffinMate.API.ApiRespons;
using TiffinMate.BLL.DTOs.NotificationDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces.NotificationInterface;

namespace TiffinMate.API.Controllers.NotificationController
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("notification")]
        public async Task<IActionResult> GetNotificationadmin( string recipienttype)
        {
            var result = await _notificationService.getnotification(recipienttype);
            if (result == null || !result.Any())
            {
                return NotFound(new ApiResponse<string>("failure", "No notification found", null, HttpStatusCode.NotFound, "No notification found"
            ));

            }
            var responce = new ApiResponse<List<NotificationAdminResponceDto>>("success", " admin notifiaction retrieved successfully", result, HttpStatusCode.OK, "");
            return Ok(responce);
        }
    }
}
