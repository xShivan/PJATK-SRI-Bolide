using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sri.Bolid.Shared
{
    [Serializable]
    public class PitStopRequest
    {
        public static byte[] Serialize(PitStopRequest pitStopRequest)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, pitStopRequest);
                stream.Flush();
                stream.Position = 0;
                return stream.ToArray();
            }
        }

        public static PitStopRequest Deserialize(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (PitStopRequest)formatter.Deserialize(stream);
            }
        }
    }
}
