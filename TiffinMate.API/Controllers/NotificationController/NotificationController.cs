using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiffinMate.BLL.DTOs.NotificationDTOs;
using TiffinMate.BLL.Interfaces.NotificationInterface;
using TiffinMate.BLL.Services.NotificationSevice.cs;

namespace TiffinMate.API.Controllers.NotificationController
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        public readonly NotificationService _notificationService;
        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationRequestDTO requestDTO)
        {
            if (requestDTO == null || string.IsNullOrEmpty(requestDTO.Title) || string.IsNullOrEmpty(requestDTO.Message))
            {
                return BadRequest("Invalid notification data.");
            }

            // Call the NotificationService to create and send the notification
            await _notificationService.CreateAndSendNotificationAsync(requestDTO.Title, requestDTO.Message);

            return Ok(new { message = "Notification sent successfully." });
        }
    }
}
