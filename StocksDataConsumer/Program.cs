using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using StocksDataConsumer.Services;

public class Program
{

    public static async Task Main(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Setup dependency injection
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<IConnectionFactory, ConnectionFactory>(sp => new ConnectionFactory() { HostName = "localhost" })

            .AddSingleton<IStocksDataConsumerService, StocksDataConsumerService>()
            .BuildServiceProvider();

        // Resolve and run the publisher
        var publisher = serviceProvider.GetService<IStocksDataConsumerService>();
        await publisher.StartListening();


        await Task.Delay(-1); // Task.Delay(-1) effectively runs forever

    }
}
