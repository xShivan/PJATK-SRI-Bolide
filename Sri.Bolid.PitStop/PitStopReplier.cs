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
        private bool isConsuming = false;

        public PitStopReplier()
        {

        }

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
                    Console.WriteLine(" [x] Awaiting RPC requests");


                    consumer.Received += (model, ea) =>
                    {
                        Console.WriteLine("PIT STOP REQUEST RECEIVED!!!");
                        PitStopRequestReply response = null;

                        var body = ea.Body;
                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();
                        replyProps.CorrelationId = props.CorrelationId;

                        var pitStopRequest = PitStopRequest.Deserialize(body);
                        response = new PitStopRequestReply(); // TODO: put some params inside

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