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

        public WarningLevel Analyze(CarParams carParams)
        {
            WarningLevel pressureExceeded = WarningLevel.None, engineTempExceeded = WarningLevel.None, radiatorFluidTempExceeded = WarningLevel.None;

            if (carParams.TyresPressure > lv1WarningPressure)
            {
                pressureExceeded = WarningLevel.Lv1;
                if (carParams.TyresPressure > lv2WarningPressure) pressureExceeded = WarningLevel.Lv2;
            }

            if (carParams.EngineTemperature > lv1WarningTemperature)
            {
                engineTempExceeded = WarningLevel.Lv1;
                if (carParams.EngineTemperature > lv2WarningTemperature) engineTempExceeded = WarningLevel.Lv2;
            }

            if (carParams.RadiatorFluidTemperature > lv1WarningTemperature)
            {
                radiatorFluidTempExceeded = WarningLevel.Lv1;
                if (carParams.RadiatorFluidTemperature > lv2WarningTemperature) radiatorFluidTempExceeded = WarningLevel.Lv2;
            }

            if (pressureExceeded != WarningLevel.None || engineTempExceeded != WarningLevel.None || radiatorFluidTempExceeded != WarningLevel.None)
            {
                this.PrintCarParam(pressureExceeded, $"Tyres press.: {carParams.TyresPressure}");
                this.PrintCarParam(radiatorFluidTempExceeded, $"Radiator fluid temp.: {carParams.RadiatorFluidTemperature}");
                this.PrintCarParam(engineTempExceeded, $"Engine temp press.: {carParams.EngineTemperature}");
                Console.WriteLine();
                return (WarningLevel)Math.Max((int)pressureExceeded, Math.Max((int)engineTempExceeded, (int)radiatorFluidTempExceeded));
            }

            return WarningLevel.None;
        }

        private void PrintCarParam(WarningLevel warningLevel, string text)
        {
            switch (warningLevel)
            {
                case WarningLevel.Lv1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case WarningLevel.Lv2:
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
