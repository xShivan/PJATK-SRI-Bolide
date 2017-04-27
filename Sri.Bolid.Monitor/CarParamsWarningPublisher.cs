using RabbitMQ.Client;
using Sri.Bolid.Shared;
using System;

namespace Sri.Bolid.Monitor
{
    public class CarParamsWarningPublisher
    {
        public void Publish(WarningLevel warningLevel)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "car_health", type: "topic");
                    channel.BasicPublish(exchange: "car_health",
                        routingKey: $"warning.{warningLevel}",
                        basicProperties: null,
                        body: null);
                    Console.WriteLine($"{warningLevel} warning published!");
                }
            }
        }
    }
}
