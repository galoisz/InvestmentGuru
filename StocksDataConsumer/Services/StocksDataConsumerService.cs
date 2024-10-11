using DataConsumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace StocksDataConsumer.Services;

public class DailyPrice
{
    public string Date { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public long Volume { get; set; }
}

public class StocksDataConsumerService : IStocksDataConsumerService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public StocksDataConsumerService(IConnectionFactory connectionFactory)
    {
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
            var stockEntity = JsonSerializer.Deserialize<StockEntry>(message);

            //await GetDailyPrices(stockEntity.Symbol, stockEntity.FromDate.ToString("yyyy-MM-dd"), stockEntity.ToDate.ToString("yyyy-MM-dd"));
            await GetDailyPrices(stockEntity.Symbol, "2024-01-01", "2024-10-01");



            Console.WriteLine(" [x] Received '{0}':'{1}'", ea.RoutingKey, message);

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


    public async Task GetDailyPrices(string symbol, string fromDate, string toDate)
    {
        HttpClient  _httpClient = new HttpClient();
        string _financeApiBaseUrl = "http://localhost:3000/finance/daily-prices";
        var url = $"{_financeApiBaseUrl}?symbol={symbol}&startDate={fromDate}&endDate={toDate}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);

            var prices = JsonSerializer.Deserialize<List<DailyPrice>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });


        }
        catch (Exception ex)
        {

            Console.WriteLine($"Error: {ex}");
        }

        //return prices ?? new List<DailyPrice>();
    }
}
