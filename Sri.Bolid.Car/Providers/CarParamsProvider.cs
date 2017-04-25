using System;
using Sri.Bolid.Shared;

namespace Sri.Bolid.Car.Providers
{
    public class CarParamsProvider
    {
        private readonly Random random = new Random();

        public CarParams Get()
        {
            return new CarParams()
            {
                EngineTemperature = RandomizeEngineTemperature(),
                RadiatorFluidTemperature = RandomizeRadiatorFluidTemperature(),
                TyresPressure = RandomizeTyresPressure()
            };
        }

        private decimal RandomizeRadiatorFluidTemperature() => this.random.Next(50, 150);

        private decimal RandomizeEngineTemperature() => this.random.Next(50, 150);

        private decimal RandomizeTyresPressure()
        {
            decimal baseTyrePressure = 2;
            int randomDecimal = this.random.Next(3, 10);
            baseTyrePressure += Convert.ToDecimal(randomDecimal * 0.1);
            return baseTyrePressure;
        }
    }
}
