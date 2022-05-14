using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace UserServiceWebApi.Service
{
    public class QueryPublisherService
    {
        private readonly ILogger<UserService> _logger;
        private IConnection _connection;
        private IConnection GetConnection() =>
            (_connection != null && _connection.IsOpen) ? 
            _connection : 
            _connection = new ConnectionFactory() { HostName = "localhost" }.CreateConnection();
        public QueryPublisherService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Send message to queue by RabbitMQ
        /// </summary>
        /// <param name="obj">Object message what will be sended</param>
        public void SendMessage(object obj)
        {
            try
            {
                var message = JsonSerializer.Serialize(obj);
                SendMessage(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"QueryPublisherService.SendMessage (obj) occures error by {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Send message to queue by RabbitMQ
        /// </summary>
        /// <param name="message">String message what will be sended</param>
        public void SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;
            using (var connection = GetConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "user-queue", exclusive: false, durable: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: "user-queue",
                    basicProperties: null,
                    body: body
                );
            }
        }
    }
}
