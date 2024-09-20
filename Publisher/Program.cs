using System.Text;
using System.Timers;
using RabbitMQ.Client;

class Program
{
    private static IModel _channel;
    private static IBasicProperties _properties;
    private static readonly Random _random = new Random();
    static async Task Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        _channel = connection.CreateModel();

        _channel.QueueDeclare(queue: "task_queue",
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        _properties = _channel.CreateBasicProperties();
        _properties.Persistent = true;

        var timer = new System.Timers.Timer(100);
        timer.Elapsed += async (sender, e) => await PublishRandomMessage();
        timer.Start();

        Console.WriteLine("Press [enter] to exit.");
        Console.ReadLine();

        timer.Stop();
        timer.Dispose();
    }

    private static async Task PublishRandomMessage()
    {
        var message = GenerateRandomMessage();
        var body = Encoding.UTF8.GetBytes(message);

        await Task.Run(() =>
        {
            _channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "task_queue",
                                 basicProperties: _properties,
                                 body: body);
        });

        Console.WriteLine($" [x] Sent {message}");
    }

    private static string GenerateRandomMessage()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 10)
                                    .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}
