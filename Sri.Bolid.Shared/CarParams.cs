using Humanizer;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sri.Bolid.Shared
{
    [Serializable]
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

        public static byte[] Serialize(CarParams carParams)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, carParams);
                stream.Flush();
                stream.Position = 0;
                return stream.ToArray();
            }
        }

        public static CarParams Deserialize(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (CarParams)formatter.Deserialize(stream);
            }
        }
    }
}
