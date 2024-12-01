using System.Text;
using FishingCatalog.msCart.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FishingCatalog.msCart.MessageBroker
{
    public class RabbitMQService(CartRepository cartRepository) : IDisposable
    {
        private readonly CartRepository _cartRepository = cartRepository;
        private IConnection _connection;
        private IChannel _cartChannel;

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
                    userId = Guid.Parse(messageJson);
                    var res = await _cartRepository.DeleteAllByUserId(userId);
                    if (res != Guid.Empty)
                    {
                        Console.WriteLine("Carts were removed successfully");
                    }
                    else
                    {
                        Console.WriteLine("Error to delete carts");
                    }
                }
                catch
                {
                    Console.WriteLine("Error to parse Guid");
                }
                
            };

            channel.BasicConsumeAsync(queue:   queueName,
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
