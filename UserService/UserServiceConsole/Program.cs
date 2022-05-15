using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using UserServiceConsole.DbContexts;
using UserService.Console.Service;
using UserService.Console.Services;
using Microsoft.Extensions.Hosting;

namespace UserServiceConsole
{
    public class Program
    {
        public static IConfigurationRoot configuration;
        static void Main(string[] args)
        {
            Console.WriteLine($"Console application ({Environment.Version.ToString()}) was started\n{DateTime.UtcNow.ToShortTimeString()}");
            CreateHostBuilder(args).Build().Run();
            Console.ReadLine();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                      
            .ConfigureServices((hostContext, services) =>
            {
                configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                    .AddJsonFile("appsettings.json", false)
                    .Build();
                services.AddSingleton<IConfigurationRoot>(configuration);
                services.AddSingleton<MongoDbContext>();
                services.AddScoped<UsersService>();
                services.AddHostedService<QueueService>();
        });
    }
}
