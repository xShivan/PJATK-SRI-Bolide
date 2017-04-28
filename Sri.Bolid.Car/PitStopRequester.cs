using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sri.Bolid.Shared;

namespace Sri.Bolid.Car
{
    public class PitStopRequester
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly QueueingBasicConsumer consumer;

        public PitStopRequester()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queue: replyQueueName,
                noAck: true,
                consumer: consumer);
        }

        public PitStopRequestReply Request(PitStopRequest request)
        {
            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            channel.BasicPublish(exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: PitStopRequest.Serialize(request));

            while (true)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                    return PitStopRequestReply.Deserialize(ea.Body);
            }
        }

        public void Stop()
        {
            this.connection.Close();
        }
    }
}
