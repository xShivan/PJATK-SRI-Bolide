using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace Sri.Bolid.Shared
{
    public class CarParamsWarningConsumer
    {
        private readonly EventHandler<BasicDeliverEventArgs> paramsReceived;

        private bool isConsuming = false;

        public CarParamsWarningConsumer(EventHandler<BasicDeliverEventArgs> paramsReceivedEventHandler)
        {
            this.paramsReceived = paramsReceivedEventHandler;
        }

        public void Stop()
        {
            this.isConsuming = false;
        }

        public void Consume()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "car_health", type: "topic");
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                        exchange: "car_health",
                        routingKey: "warning.*");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += this.paramsReceived;
                    channel.BasicConsume(queue: queueName,
                        noAck: true,
                        consumer: consumer);
                    this.isConsuming = true;

                    while (isConsuming) { Thread.Sleep(1); }
                }
            }
        }
    }
}
