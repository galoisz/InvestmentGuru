using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Channels;

class Consumer
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

            // Declare a durable queue
            string queueName = $"durable_queue_{new Random().Next(100)}";
            //var queueName = channel.QueueDeclare().QueueName;
            channel.QueueDeclare(
                queue: queueName,
                durable: true,    // Make the queue durable
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            // Bind the queue to the exchange
            string routingKey = "my_routing_key";
            channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey
            );

            // Create a consumer to listen to the queue
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Received '{message}'");
            };

            // Start consuming
            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
