using RabbitMQ.Client;
using Sri.Bolid.Shared;
using System;

namespace Sri.Bolid.Monitor
{
    public class CarParamsWarningPublisher
    {
        public void Publish(Warning warning)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "car_health", type: "topic");
                    channel.BasicPublish(exchange: "car_health",
                        routingKey: $"warning.{GetWarningLevel(warning)}",
                        basicProperties: null,
                        body: Warning.Serialize(warning));
                    Console.WriteLine($"{GetWarningLevel(warning)} warning published!");
                }
            }
        }

        private WarningLevel GetWarningLevel(Warning warning)
        {
            return (WarningLevel)Math.Max((int)warning.EngineTempWarningLevel, Math.Max((int)warning.RadiatorFluidTempWarningLevel, (int)warning.TyresPressureWarningLevel));
        }
    }
}
