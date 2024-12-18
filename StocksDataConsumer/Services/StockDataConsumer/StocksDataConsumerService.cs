using DataConsumer.Services.StockDataConsumer;
using DataConsumer.Services.StockPrices;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Application.Stocks;
using System.Text;
using Application.Dtos;

namespace StocksDataConsumer.Services;

public class StocksDataConsumerService : IStocksDataConsumerService
{
    private readonly IStockPriceService _stockPriceService;
    private readonly IMediator _mediator;

    public StocksDataConsumerService(IStockPriceService stockPriceService, IMediator mediator)
    {
        _stockPriceService = stockPriceService;
        _mediator = mediator;
    }

    public async Task StartListening()
    {

        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "stock_exchange",
            type: ExchangeType.Direct,
            durable: true);


        var queueName = "NASDAQ";

        await channel.QueueDeclareAsync(queue: queueName,
                              durable: true,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

        await channel.QueueBindAsync(queue: queueName, exchange: "stock_exchange", routingKey: queueName);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {

            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received '{0}':'{1}'", ea.RoutingKey, message);

                var stockEntity = JsonConvert.DeserializeObject<DataConsumer.Models.StockEntry>(message);

                var prices = await _stockPriceService.GetStockPrices(stockEntity.Symbol, new DateTime(1980, 01, 1), DateTime.Now);
                var command = new StockUpsertCommand
                {
                    Symbol = stockEntity.Symbol,
                    Prices = prices.Select(x => new PriceDto { Open = Convert.ToDecimal(x.Open), High = Convert.ToDecimal(x.High), Close = Convert.ToDecimal(x.Close), Low = Convert.ToDecimal(x.Low), Date = x.Date.ToString("yyyy-MM-dd"), Volume = Convert.ToInt64(x.Volume) }).ToList()
                };

                await _mediator.Send(command);

                Console.WriteLine(JsonConvert.SerializeObject(prices));

                // Acknowledge the message
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();
    }
}
