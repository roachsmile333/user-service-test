using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace UserServiceWebApi.Service
{
    public class QueryPublisherService
    {
        private readonly ILogger<UsersService> _logger;
        private IConnection _connection;
        private readonly IConfiguration _configuration;
        private IConnection GetConnection() =>
            (_connection != null && _connection.IsOpen) ? 
            _connection : 
            _connection = new ConnectionFactory() { HostName = "localhost" }.CreateConnection();
        public QueryPublisherService(ILogger<UsersService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Send command to queue by RabbitMQ to store user 
        /// </summary>
        /// <param name="username">String username what will be stored</param>
        public void SendCommand(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return;
                using (var connection = GetConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _configuration.GetSection("RabbitMQ").GetSection("HostName").Value,
                        exclusive: false, durable: false, autoDelete: false, arguments: null);
                    var body = Encoding.UTF8.GetBytes(username);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: _configuration.GetSection("RabbitMQ").GetSection("HostName").Value,
                        basicProperties: null,
                        body: body
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SendCommand occurred error by next:\n {ex.Message}");
                throw;
            }
        }
    }
}
