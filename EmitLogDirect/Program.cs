using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

class EmitLogsDirect
{
    static void Main(string[] args)
    {
        // Initialize the connection factory with the RabbitMQ server details
        var factory = new ConnectionFactory { HostName = "localhost" };

        // Establish the connection and create a channel
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        // Declare a direct exchange named "direct_logs"
        channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

        // Define the severities you want to publish
        var severities = new[] { "info", "warning", "error" };

        Console.WriteLine(" [*] Publishing messages every 500ms. Press [CTRL+C] to exit.");

        // Infinite loop to publish messages periodically
        while (true)
        {
            foreach (var severity in severities)
            {
                // Create the message content
                var message = $"Log {severity} at {DateTime.Now:O}";
                var body = Encoding.UTF8.GetBytes(message);

                // Publish the message to the "direct_logs" exchange with the appropriate routing key
                channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: severity,
                                     basicProperties: null,
                                     body: body);

                // Output to console for confirmation
                Console.WriteLine($" [x] Sent '{severity}':'{message}'");
            }

            // Wait for 500 milliseconds before sending the next set of messages
            Thread.Sleep(500);
        }

        // Note: The following lines are unreachable due to the infinite loop.
        // If you plan to add a termination condition, you can handle it accordingly.
        // Console.WriteLine(" Press [enter] to exit.");
        // Console.ReadLine();
    }
}
