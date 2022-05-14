using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace UserServiceConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Console application ({Environment.Version.ToString()}) was started\n{DateTime.UtcNow.ToShortTimeString()}");

            try
            {
                ConsumerConnection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void ConsumerConnection()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "user-queue");
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine($"{message}\n{DateTime.UtcNow.ToShortTimeString()}");
                };
            }
        }
    }
}
