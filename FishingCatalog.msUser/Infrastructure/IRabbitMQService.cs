using FishingCatalog.Core;

namespace FishingCatalog.msUser.Infrastructure
{
    public interface IRabbitMQService
    {
        void Dispose();
        Task InitializeAsync();
        Task SendGuidAsync(Guid id);
        Task SendMessageAsync(EmailMessage emailMessage);
    }
}