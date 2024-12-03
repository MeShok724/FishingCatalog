using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FishingCatalog.Core;
using FishingCatalog.msNotification.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace FishingCatalog.msNotification.MessageBroker
{
    public class RabbitMQService(IEmailService emailService) : IDisposable
    {
        private IConnection _connection;
        private IChannel _feedbackChannel;
        private readonly IEmailService _emailService = emailService;

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

                // Десериализуем сообщение в объект EmailMessage
                var emailMessage = JsonConvert.DeserializeObject<EmailMessage>(messageJson);

                if (emailMessage != null)
                {
                    Console.WriteLine($"Email: {emailMessage.Email}");
                    Console.WriteLine($"Subject: {emailMessage.Subject}");
                    Console.WriteLine($"Text: {emailMessage.Text}");

                    await _emailService.SendEmailAsync(emailMessage.Email, emailMessage.Subject, emailMessage.Text);
                }
            };

            channel.BasicConsumeAsync(queue: "registrationQueue",
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
