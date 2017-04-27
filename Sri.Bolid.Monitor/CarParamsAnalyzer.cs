using Sri.Bolid.Shared;
using System;

namespace Sri.Bolid.Monitor
{
    public class CarParamsAnalyzer
    {
        private decimal lv1WarningTemperature => Convert.ToDecimal((double)CarParams.MaxTemperature * 0.85);

        private decimal lv2WarningTemperature => Convert.ToDecimal((double)CarParams.MaxTemperature * 0.95);

        private decimal lv1WarningPressure => Convert.ToDecimal((double)CarParams.MaxPressure * 0.8);

        private decimal lv2WarningPressure => Convert.ToDecimal((double)CarParams.MaxPressure * 0.9);

        public Warning Analyze(CarParams carParams)
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
                CarParams.Print(pressureExceeded, $"Tyres press.: {carParams.TyresPressure}");
                CarParams.Print(radiatorFluidTempExceeded, $"Radiator fluid temp.: {carParams.RadiatorFluidTemperature}");
                CarParams.Print(engineTempExceeded, $"Engine temp press.: {carParams.EngineTemperature}");
                Console.WriteLine();
                //return (WarningLevel)Math.Max((int)pressureExceeded, Math.Max((int)engineTempExceeded, (int)radiatorFluidTempExceeded));
            }

            return new Warning()
            {
                CarParams = carParams,
                EngineTempWarningLevel = engineTempExceeded,
                RadiatorFluidTempWarningLevel = radiatorFluidTempExceeded,
                TyresPressureWarningLevel = pressureExceeded
            };
        }
    }
}
