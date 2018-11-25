
using AplicationInformationExchange.Model;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AplicationInformationExchange.Serialization
{
    public class BinarySerialization : ISerialization
    {
        public Message Deserialize(byte[] content)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(content);
            return (Message)formatter.Deserialize(stream);
        }

        public byte[] Serialize(Message message)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, message);
            return stream.ToArray();
        }
    }
}
