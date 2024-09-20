using System;
using System.Text;
using RabbitMQ.Client;

class Program
{
    static void Main(string[] args)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

        Console.WriteLine("Enter messages to send to the exchange. Type 'exit' to quit.");

        while (true)
        {
            var message = Console.ReadLine();

            if (message?.ToLower() == "exit")
                break;

            var body = Encoding.UTF8.GetBytes(message ?? "info: Hello World!");
            channel.BasicPublish(exchange: "logs",
                                 routingKey: string.Empty,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($" [x] Sent {message}");
        }

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}
