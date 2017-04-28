using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sri.Bolid.Shared;

namespace Sri.Bolid.PitStop
{
    class PitStopReplier
    {
        private Random random = new Random();

        private bool isConsuming = false;

        public void Reply()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "rpc_queue", durable: false,
                        exclusive: false, autoDelete: false, arguments: null);
                    channel.BasicQos(0, 1, false);
                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(queue: "rpc_queue",
                        noAck: false, consumer: consumer);

                    consumer.Received += (model, ea) =>
                    {
                        var pitStopRequest = PitStopRequest.Deserialize(ea.Body);

                        Console.Write($"Stop request received: [{pitStopRequest.Message}] - ");
                        bool accept = this.random.Next(0, 10) > 5;
                        if (accept)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Accepted");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Rejected");
                        }
                        Console.ForegroundColor = ConsoleColor.Gray;


                        PitStopRequestReply response = new PitStopRequestReply() { IsAccepted = accept };

                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;

                        channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                            basicProperties: replyProps, body: PitStopRequestReply.Serialize(response));
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                            multiple: false);
                    };

                    this.isConsuming = true;
                    while (isConsuming) { Thread.Sleep(1); }
                }
            }
        }

        public void Stop()
        {
            this.isConsuming = false;
        }
    }
}