using System;
using Humanizer;

namespace Sri.Bolid.Shared
{
    public class CarParams
    {
        public TimeSpan RaceTime { get; set; }

        public decimal TyresPressure { get; set; }

        public decimal RadiatorFluidTemperature { get; set; }

        public decimal EngineTemperature { get; set; }

        public override string ToString()
        {
            string carParams = $"Tyres press.: {TyresPressure} Radiator fluid temp.: {RadiatorFluidTemperature} Engine temp.: {EngineTemperature}";
            var carParamsFull = this.RaceTime.Milliseconds == 0 ? carParams : $"[{this.RaceTime.Humanize()}] {carParams}";
            return carParamsFull;
        }
    }
}
