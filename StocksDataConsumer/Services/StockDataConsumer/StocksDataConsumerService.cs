using DataConsumer.Models;
using DataConsumer.Services.StockDataConsumer;
using DataConsumer.Services.StockPrices;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace StocksDataConsumer.Services;

public class StocksDataConsumerService : IStocksDataConsumerService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
     
    private readonly IStockPriceService _stockPriceService;
    public StocksDataConsumerService(IConnectionFactory connectionFactory, IStockPriceService stockPriceService)
    {
        _stockPriceService = stockPriceService;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: "stock_exchange", 
            type: ExchangeType.Direct,
            durable: true);
    }

    public async Task StartListening()
    {
        var queueName = "NASDAQ";//_channel.QueueDeclare().QueueName;
                                 //_channel.QueueDeclare( durable: true, exclusive: false, autoDelete: false, arguments: null);

        //_channel.QueueBind(queue: queueName,
        //                   exchange: "stock_exchange",
        //                   routingKey: "NASDAQ");


       _channel.QueueDeclare(queue: queueName,
                        durable: true, // Queue is durable
                        exclusive: false,
                        autoDelete: false,
        arguments: null);

        _channel.QueueBind(queue: queueName, exchange: "stock_exchange", routingKey: queueName);


        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received '{0}':'{1}'", ea.RoutingKey, message);

            var stockEntity = JsonConvert.DeserializeObject<StockEntry>(message);

            var prices = await _stockPriceService.GetStockPrices(stockEntity.Symbol, new DateTime(2024, 1, 1), DateTime.Now);

            Console.WriteLine(JsonConvert.SerializeObject(prices));        
            // Perform some operation
            // For example: Console.WriteLine("Processing stock: {0}", stockEntity.StockName);

            // Acknowledge the message
            _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        };

        _channel.BasicConsume(queue: queueName,
                              autoAck: false,
                              consumer: consumer);

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();

    }
}
