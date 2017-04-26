using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sri.Bolid.Shared;
using System.Configuration;

namespace Sri.Bolid.Logger
{
    class Program
    {
        private static TextLogger textLogger = new TextLogger(ConfigurationManager.AppSettings["LogFilename"]);

        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                        exchange: "topic_logs",
                        routingKey: "car.info");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += HandleCarParamsReceived;
                    channel.BasicConsume(queue: queueName,
                        noAck: true,
                        consumer: consumer);

                    Console.WriteLine("Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        private static void HandleCarParamsReceived(object model, BasicDeliverEventArgs ea)
        {
            CarParams carParams = CarParams.Deserialize(ea.Body);
            Console.WriteLine("Received car params:" + carParams.ToString());
            textLogger.AppendLine(carParams.ToString());
        }
    }
}
