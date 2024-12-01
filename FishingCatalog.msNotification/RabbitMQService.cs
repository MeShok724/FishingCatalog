using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FishingCatalog.msNotification
{
    public class RabbitMQService:IDisposable
    {
        private IConnection _connection;
        private IChannel _registrationChannel;
        private IChannel _feedbackChannel;

        public async Task InitializeAsync()
        {
            // Создаем фабрику подключения
            var factory = new ConnectionFactory { HostName = "localhost" };

            // Устанавливаем соединение (одно для всех каналов)
            _connection = await factory.CreateConnectionAsync();

            // Канал для очереди "registrationQueue"
            _registrationChannel = await _connection.CreateChannelAsync();
            await _registrationChannel.QueueDeclareAsync(queue: "registrationQueue",
                                              durable: false,
                                              exclusive: false,
                                              autoDelete: false,
                                              arguments: null);

            // Канал для очереди "feedbackQueue"
            _feedbackChannel = await _connection.CreateChannelAsync();
            await _feedbackChannel.QueueDeclareAsync(queue: "feedbackQueue",
                                          durable: false,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);

        }

        public void StartListening()
        {
            StartListeningToQueue(_registrationChannel, "registrationQueue");
            StartListeningToQueue(_feedbackChannel, "feedbackQueue");
        }

        private void StartListeningToQueue(IChannel channel, string queueName)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[{queueName}] Received: {message}");

                // Здесь обработайте сообщение
                return Task.CompletedTask;
            };

            channel.BasicConsumeAsync(queue: "registrationQueue",
                                               autoAck: true,
                                               consumer: consumer);
        }

        public void Dispose()
        {
            _registrationChannel?.Dispose();
            _feedbackChannel?.Dispose();
            _connection?.Dispose();
        }
    }
}
