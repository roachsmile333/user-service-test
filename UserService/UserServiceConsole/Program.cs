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

            //TODO: move consumer read block to background worker
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "user-queue", exclusive: false, durable: false, autoDelete: false, arguments: null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body.ToArray());
                        Console.WriteLine($"{message}\n{DateTime.UtcNow.ToShortTimeString()}");
                    };
                    channel.BasicConsume(autoAck: true, queue: "user-queue", consumer: consumer);
                    Console.ReadLine();
                }                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
