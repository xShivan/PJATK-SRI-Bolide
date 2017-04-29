using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Sri.Bolid.Shared
{
    public class CarParamsConsumer
    {
        private readonly EventHandler<BasicDeliverEventArgs> paramsReceived;

        private bool isConsuming = false;

        public CarParamsConsumer(EventHandler<BasicDeliverEventArgs> paramsReceivedEventHandler)
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
                    channel.ExchangeDeclare(exchange: "car_info", type: "fanout");
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                        exchange: "car_info",
                        routingKey: "");

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
