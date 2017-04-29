using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sri.Bolid.Shared
{
    [Serializable]
    public class Warning
    {
        public CarParams CarParams { get; set; }

        public WarningLevel TyresPressureWarningLevel { get; set; }

        public WarningLevel RadiatorFluidTempWarningLevel { get; set; }

        public WarningLevel EngineTempWarningLevel { get; set; }

        public static byte[] Serialize(Warning warning)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, warning);
                stream.Flush();
                stream.Position = 0;
                return stream.ToArray();
            }
        }

        public static Warning Deserialize(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (Warning)formatter.Deserialize(stream);
            }
        }
    }
}
