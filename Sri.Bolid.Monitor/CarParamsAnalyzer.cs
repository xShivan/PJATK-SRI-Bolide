using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Sri.Bolid.Shared;

namespace Sri.Bolid.Monitor
{
    public class CarParamsAnalyzer
    {
        private decimal lv1WarningTemperature => Convert.ToDecimal((double)CarParams.MaxTemperature * 0.85);

        private decimal lv2WarningTemperature => Convert.ToDecimal((double)CarParams.MaxTemperature * 0.95);

        private decimal lv1WarningPressure => Convert.ToDecimal((double)CarParams.MaxPressure * 0.8);

        private decimal lv2WarningPressure => Convert.ToDecimal((double)CarParams.MaxPressure * 0.9);

        public void Analyze(CarParams carParams)
        {
            short pressureExceeded = 0, engineTempExceeded = 0, radiatorFluidTempExceeded = 0;
            if (carParams.TyresPressure > lv1WarningPressure)
            {
                pressureExceeded = 1;
                if (carParams.TyresPressure > lv2WarningPressure) pressureExceeded = 2;
            }

            if (carParams.EngineTemperature > lv1WarningTemperature)
            {
                engineTempExceeded = 1;
                if (carParams.EngineTemperature > lv2WarningTemperature) engineTempExceeded = 2;
            }

            if (carParams.RadiatorFluidTemperature > lv1WarningTemperature)
            {
                radiatorFluidTempExceeded = 1;
                if (carParams.RadiatorFluidTemperature > lv2WarningTemperature) radiatorFluidTempExceeded = 2;
            }

            if (pressureExceeded != 0 || engineTempExceeded != 0 || radiatorFluidTempExceeded != 0)
            {
                this.PrintCarParam(pressureExceeded, $"Tyres press.: {carParams.TyresPressure}");
                this.PrintCarParam(radiatorFluidTempExceeded, $"Radiator fluid temp.: {carParams.RadiatorFluidTemperature}");
                this.PrintCarParam(engineTempExceeded, $"Engine temp press.: {carParams.EngineTemperature}");
                Console.WriteLine();
            }
        }

        private void PrintCarParam(short isExceeded, string text)
        {
            switch (isExceeded)
            {
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            Console.Write(text);
            Console.Write(" ");

            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
