using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sri.Bolid.Car.Providers;
using Sri.Bolid.Shared;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Sri.Bolid.Car
{
    class Program
    {
        private const int broadcastTimerInterval = 2000;

        private const int raceTimerInterval = 10;

        private static readonly CarParamsProvider carParamsProvider = new CarParamsProvider();
        private static readonly Consumer carParamsWarningConsumer = ConsumerFactory.Create("car_health", "topic", "warning.*", ParamsReceivedEventHandler);

        private static long raceMiliseconds = 0;

        static void Main(string[] args)
        {
            Task.Run(() => carParamsWarningConsumer.Consume());
            StartTimers();

            while (true)
            {
                Thread.Sleep(1);
            }
        }

        private static void StartTimers()
        {
            Timer raceTimer = new Timer(raceTimerInterval);
            raceTimer.Elapsed += (sender, eventArgs) => raceMiliseconds += raceTimerInterval;
            raceTimer.Start();

            Timer broadcastTimer = new Timer(broadcastTimerInterval);
            broadcastTimer.Elapsed += BroadcastCarParams;
            broadcastTimer.Start();
        }

        private static void BroadcastCarParams(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            CarParams carParams = carParamsProvider.Get();
            carParams.RaceTime = TimeSpan.FromMilliseconds(raceMiliseconds);
            Console.WriteLine(carParams.ToString());

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "car_info",
                        type: "fanout");

                    channel.BasicPublish(exchange: "car_info",
                        routingKey: "",
                        basicProperties: null,
                        body: CarParams.Serialize(carParams));
                }
            }
        }

        private static void ParamsReceivedEventHandler(object o, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            Warning warning = Warning.Deserialize(basicDeliverEventArgs.Body);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Warning received: ");
            CarParams.Print(warning.TyresPressureWarningLevel, $"Tyres pressure - {warning.TyresPressureWarningLevel}");
            CarParams.Print(warning.RadiatorFluidTempWarningLevel, $"Radiator fluid temperature - {warning.RadiatorFluidTempWarningLevel}");
            CarParams.Print(warning.EngineTempWarningLevel, $"Engine temperature - {warning.EngineTempWarningLevel}");
            Console.WriteLine();
        }
    }
}
