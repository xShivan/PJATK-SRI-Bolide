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

        private decimal RandomizeRadiatorFluidTemperature() => this.random.Next((int)CarParams.MinTemperature, (int)CarParams.MaxTemperature);

        private decimal RandomizeEngineTemperature() => this.random.Next((int)CarParams.MinTemperature, (int)CarParams.MaxTemperature);

        private decimal RandomizeTyresPressure()
        {
            decimal baseTyrePressure = CarParams.MinPressure;
            int randomDecimal = this.random.Next(0, 15);
            baseTyrePressure += Convert.ToDecimal(randomDecimal * 0.1);
            return baseTyrePressure;
        }
    }
}
