using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using UserServiceConsole.DbContexts;
using UserService.Console.Service;
using UserService.Console.Services;

namespace UserServiceConsole
{
    internal class Program
    {
        public static IConfigurationRoot configuration;
        static void Main(string[] args)
        {
            Console.WriteLine($"Console application ({Environment.Version.ToString()}) was started\n{DateTime.UtcNow.ToShortTimeString()}");
            Console.ReadLine();
        }
        private static void ConfigureServices(IServiceCollection serviceCollection)
        {

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
            serviceCollection.AddSingleton<MongoDbContext>();
            serviceCollection.AddScoped<UsersService>();
            serviceCollection.AddHostedService<QueueService>();
        }
    }
}
