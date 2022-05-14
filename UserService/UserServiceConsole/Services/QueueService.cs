using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserService.Console.Service;

namespace UserService.Console.Services
{
    public class QueueService : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly UsersService _usersService;
        private readonly IConfiguration _configuration;

        public QueueService(UsersService usersService, IConfiguration configuration)
        {
            _usersService = usersService;
            _configuration = configuration;

            var factory = new ConnectionFactory { HostName = _configuration.GetSection("RabbitMQ").GetSection("HostName").Value };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _configuration.GetSection("RabbitMQ").GetSection("Queue").Value, 
                exclusive: false, durable: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                _usersService.CreateUserAsync(content)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_configuration.GetSection("RabbitMQ").GetSection("Queue").Value, false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
