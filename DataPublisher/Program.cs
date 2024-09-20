using DataPublisher.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{

    public static async  Task Main(string[] args)
    {
        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Setup dependency injection
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<IStocksDataService, StocksDataService>()
            .AddSingleton<IStocksPublisherService, StocksPublisherService>()
            .BuildServiceProvider();

        // Resolve and run the publisher
        var publisher = serviceProvider.GetService<IStocksPublisherService>();
        await publisher.PublishStocks();

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}
