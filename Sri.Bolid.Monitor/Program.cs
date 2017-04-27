using RabbitMQ.Client.Events;
using Sri.Bolid.Shared;
using System;
using System.Threading.Tasks;

namespace Sri.Bolid.Monitor
{
    class Program
    {
        private static readonly CarParamsAnalyzer carParamsAnalyzer = new CarParamsAnalyzer();
        private static readonly CarParamsWarningPublisher carParamsWarningPublisher = new CarParamsWarningPublisher();

        static void Main(string[] args)
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
            Warning warning = carParamsAnalyzer.Analyze(carParams);

            if (warning.EngineTempWarningLevel != WarningLevel.None || warning.RadiatorFluidTempWarningLevel != WarningLevel.None || warning.TyresPressureWarningLevel != WarningLevel.None)
                carParamsWarningPublisher.Publish(warning);
        }
    }
}
