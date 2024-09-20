using RabbitMQ.Client;
using System;
using System.Text;

class Producer
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            // Declare a durable exchange
            string exchangeName = "durable_exchange";
            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true // Make the exchange durable
            );

            for (int i = 0; i < 100; i++)
            {
                string routingKey = "my_routing_key";
                string message = $"Hello, RabbitMQ! {i}";
                var body = Encoding.UTF8.GetBytes(message);

                // Make the message persistent
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                // Publish the message to the exchange
                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    basicProperties: properties, // Set message properties to persistent
                    body: body
                );


                Console.WriteLine($"[x] Sent '{message}'");

                Thread.Sleep(1000);
            }

        }
    }
}
