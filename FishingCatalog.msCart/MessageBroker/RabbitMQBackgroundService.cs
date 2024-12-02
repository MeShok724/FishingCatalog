namespace FishingCatalog.msCart.MessageBroker
{

    public class RabbitMQBackgroundService(RabbitMQService rabbitMQService) : IHostedService
    {
        private readonly RabbitMQService _rabbitMQService = rabbitMQService;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _rabbitMQService.InitializeAsync();
            _rabbitMQService.StartListening();
            Console.WriteLine("RabbitMQ listening started...");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _rabbitMQService.Dispose();
            Console.WriteLine("RabbitMQ listening stopped...");
            return Task.CompletedTask;
        }
    }
}

