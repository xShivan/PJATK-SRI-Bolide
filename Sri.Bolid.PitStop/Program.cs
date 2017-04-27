using RabbitMQ.Client.Events;
using Sri.Bolid.Shared;
using System;

namespace Sri.Bolid.PitStop
{
    class Program
    {
        private static readonly Consumer carParamsWarningConsumer = ConsumerFactory.Create("car_health", "topic", $"warning.{WarningLevel.Lv2}", ParamsReceivedEventHandler);

        static void Main(string[] args)
        {
            carParamsWarningConsumer.Consume();

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();

            carParamsWarningConsumer.Stop();
        }

        private static void ParamsReceivedEventHandler(object o, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            Warning warning = Warning.Deserialize(basicDeliverEventArgs.Body);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("SERIOUS FAULT - CREW GETTING READY: ");
            CarParams.Print(warning.TyresPressureWarningLevel, $"Tyres pressure - {warning.TyresPressureWarningLevel}");
            CarParams.Print(warning.RadiatorFluidTempWarningLevel, $"Radiator fluid temperature - {warning.RadiatorFluidTempWarningLevel}");
            CarParams.Print(warning.EngineTempWarningLevel, $"Engine temperature - {warning.EngineTempWarningLevel}");
            Console.WriteLine();
        }
    }
}
