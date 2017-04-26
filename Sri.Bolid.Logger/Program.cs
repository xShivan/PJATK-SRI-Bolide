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
        private static readonly TextLogger textLogger = new TextLogger(ConfigurationManager.AppSettings["LogFilename"]);

        private static void Main(string[] args)
        {
            var carParamsConsumer = new CarParamsConsumer(HandleCarParamsReceived);
            Task.Run(() => carParamsConsumer.Consume());

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

            carParamsConsumer.Stop();
        }

        private static void HandleCarParamsReceived(object model, BasicDeliverEventArgs ea)
        {
            CarParams carParams = CarParams.Deserialize(ea.Body);
            Console.WriteLine("Received car params:" + carParams);
            textLogger.AppendLine(carParams.ToString());
        }
    }
}
