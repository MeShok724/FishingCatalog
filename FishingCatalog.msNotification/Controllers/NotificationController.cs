using FishingCatalog.msNotification.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FishingCatalog.msNotification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(IEmailService emailService) : Controller
    {
        private readonly IEmailService _emailService = emailService;

        [HttpPost]
        public async Task<IActionResult> SendToEmailList([FromBody] List<string> strings, [FromQuery] string subject, [FromQuery] string text)
        {
            if (strings == null || strings.Count == 0)
            {
                return BadRequest("Список email не может быть пустым");
            }
            foreach (string s in strings)
            {
                await _emailService.SendEmailAsync(s, subject, text);
            }
            return Ok("Письма успешно разосланы");
        }
    }
}
