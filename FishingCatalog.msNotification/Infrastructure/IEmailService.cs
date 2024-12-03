
namespace FishingCatalog.msNotification.Infrastructure
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}