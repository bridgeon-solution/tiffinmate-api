using Microsoft.AspNetCore.Mvc;
using TiffinMate.BLL.Interfaces.NotificationInterface;

namespace TiffinMate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(string title, string message)
        {
            await _notificationService.CreateAndSendNotificationAsync(title, message);
            return Ok("Notification sent.");
        }
    }
}
