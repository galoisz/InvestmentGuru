using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataPublisher.Services;

public class StocksPublisherService : IStocksPublisherService
{
    private readonly IStocksDataService _stockService;

    public StocksPublisherService(IStocksDataService stockService)
    {
        _stockService = stockService;
    }
    public async Task PublishStocks()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(
                exchange: "stock_exchange",
                type: ExchangeType.Direct,
                durable: true
                );


            foreach (var stockEntity in await _stockService.GetStockData())
            {
                var routingKey = stockEntity.MarketName;
                var message = JsonSerializer.Serialize(stockEntity);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "stock_exchange",
                                     routingKey: routingKey,
                                     basicProperties: properties,
                                     body: body);
                Console.WriteLine(" [x] Sent '{0}':'{1}'", routingKey, message);
            }


        }

    }
}
