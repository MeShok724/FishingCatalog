using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FishingCatalog.Core;
using FishingCatalog.msFeedback.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FishingCatalog.msCart.MessageBroker
{
    public class RabbitMQService(FeedbackRepository feedbackRepository) : IDisposable
    {
        private readonly FeedbackRepository _feedbackRepository = feedbackRepository;
        private IConnection _connection;
        private IChannel _feedbackChannel;

        public async Task InitializeAsync()
        {
            // Создаем фабрику подключения
            var factory = new ConnectionFactory { HostName = "localhost" };

            // Устанавливаем соединение (одно для всех каналов)
            _connection = await factory.CreateConnectionAsync();

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
            StartListeningToQueue(_feedbackChannel, "feedbackQueue");
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
                    var res = await _feedbackRepository.DeleteAllByUserId(userId);
                    if (res != Guid.Empty)
                    {
                        Console.WriteLine("Feedbacks were removed successfully");
                    }
                    else
                    {
                        Console.WriteLine("Error to delete feedbacks");
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
            _feedbackChannel?.Dispose();
            _connection?.Dispose();
        }
    }
}
