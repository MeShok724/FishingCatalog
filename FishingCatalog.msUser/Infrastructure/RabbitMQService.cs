using FishingCatalog.Core;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace FishingCatalog.msUser.Infrastructure
{
    public class RabbitMQService:IDisposable
    {
        private IConnection _connection;
        private IChannel _registrationChannel;
        private IChannel _cartChannel;
        private IChannel _feedbackChannel;

        public async Task InitializeAsync()
        {
            // Создаем подключение и канал RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = await factory.CreateConnectionAsync();
            _registrationChannel = await _connection.CreateChannelAsync();
            _cartChannel = await _connection.CreateChannelAsync();
            _feedbackChannel = await _connection.CreateChannelAsync();

            // Объявляем очередь
            await _registrationChannel.QueueDeclareAsync(queue: "registrationQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await _cartChannel.QueueDeclareAsync(queue: "cartQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await _feedbackChannel.QueueDeclareAsync(queue: "feedbackQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public async Task SendMessageAsync(EmailMessage emailMessage)
        {
            // Сериализуем объект в строку JSON
            var message = JsonSerializer.Serialize(emailMessage);
            var messageBody = Encoding.UTF8.GetBytes(message);
            var basicProperties = new BasicProperties();

            // Отправляем сообщение в очередь
            await _registrationChannel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "registrationQueue",
                    mandatory: true,
                    basicProperties: basicProperties,
                    body: messageBody
            );

            Console.WriteLine($"Сообщение отправлено: {message}");
        }
        public async Task SendGuidAsync(Guid id)
        {
            var messageBody = Encoding.UTF8.GetBytes(id.ToString());
            var basicProperties = new BasicProperties();

            // Отправляем сообщение в очередь
            await _cartChannel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "cartQueue",
                    mandatory: true,
                    basicProperties: basicProperties,
                    body: messageBody
            );
            await _feedbackChannel.BasicPublishAsync(
                    exchange: "",
                    routingKey: "feedbackQueue",
                    mandatory: true,
                    basicProperties: basicProperties,
                    body: messageBody
            );

            Console.WriteLine($"Sent GUID: {id}");
        }

        // Метод для закрытия подключения
        public void Dispose()
        {
            _registrationChannel?.Dispose();
            _cartChannel?.Dispose();
            _feedbackChannel?.Dispose();
            _connection?.Dispose();
        }
    }
}
