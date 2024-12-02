using System.Text;
using FishingCatalog.msCart.Repositories;
using FishingCatalog.Postgres;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FishingCatalog.msCart.MessageBroker
{
    public class RabbitMQService(IServiceScopeFactory scopeFactory) : IDisposable
    {
        private IConnection _connection;
        private IChannel _cartChannel;
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        public async Task InitializeAsync()
        {
            // Создаем фабрику подключения
            var factory = new ConnectionFactory { HostName = "localhost" };

            // Устанавливаем соединение (одно для всех каналов)
            _connection = await factory.CreateConnectionAsync();

            // Канал для очереди "feedbackQueue"
            _cartChannel = await _connection.CreateChannelAsync();
            await _cartChannel.QueueDeclareAsync(queue: "cartQueue",
                                          durable: false,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);
            
        }

        public void StartListening()
        {
            StartListeningToQueue(_cartChannel, "cartQueue");
        }



        private void StartListeningToQueue(IChannel channel, string queueName)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{queueName}] Received: {messageJson}");
                Guid userId;
                try
                {
                    Console.WriteLine("Parsing");
                    userId = Guid.Parse(messageJson);
                    Console.WriteLine("Parsed successfully");
                }
                catch
                {
                    Console.WriteLine("Error to parse Guid");
                    return;
                }
                Guid res = Guid.Empty;
                using (var scope = _scopeFactory.CreateScope())
                {
                    Console.WriteLine("Getting repository");
                    var cartRepository = scope.ServiceProvider.GetRequiredService<CartRepository>();
                    Console.WriteLine("Deleting carts");
                    res = await cartRepository.DeleteAllByUserId(userId);
                    Console.WriteLine("Carts were removed");
                }
            };

            channel.BasicConsumeAsync(queue: "cartQueue",
                                               autoAck: true,
                                               consumer: consumer);
        }

        public void Dispose()
        {
            _cartChannel?.Dispose();
            _connection?.Dispose();
        }
    }
}
