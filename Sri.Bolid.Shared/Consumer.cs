using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;

namespace Sri.Bolid.Shared
{
    public class Consumer
    {
        private readonly EventHandler<BasicDeliverEventArgs> paramsReceived;

        private bool isConsuming = false;

        public string Exchange { get; private set; }

        public string Type { get; private set; }

        public string RoutingKey { get; private set; }

        public Consumer(string exchange, string type, string routingKey, EventHandler<BasicDeliverEventArgs> paramsReceived)
        {
            this.paramsReceived = paramsReceived;
            this.Exchange = exchange;
            this.Type = type;
            this.RoutingKey = routingKey;
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
                    channel.ExchangeDeclare(exchange: this.Exchange, type: this.Type);
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                        exchange: this.Exchange,
                        routingKey: this.RoutingKey);

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
