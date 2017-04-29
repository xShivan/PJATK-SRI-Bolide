using RabbitMQ.Client.Events;
using System;

namespace Sri.Bolid.Shared
{
    public class ConsumerFactory
    {
        public static Consumer Create(string exchange, string type, string routingKey, EventHandler<BasicDeliverEventArgs> paramsReceived)
        {
            return new Consumer(exchange, type, routingKey, paramsReceived);
        }
    }
}
