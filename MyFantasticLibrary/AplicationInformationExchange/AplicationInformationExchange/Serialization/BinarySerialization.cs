
using AplicationInformationExchange.Model;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AplicationInformationExchange.Serialization
{
    /// <summary>
    /// Serialization to binary format
    /// </summary>
    public class BinarySerialization : ISerialization
    {
        /// <summary>
        /// Deserialize message from byte array.
        /// </summary>
        /// <param name="content">Byte array to deserialize</param>
        /// <returns>Deserialized <see cref="Message"/></returns>
        public Message Deserialize(byte[] content)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(content);
            return (Message)formatter.Deserialize(stream);
        }
        /// <summary>
        /// Serialize message to byte array.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to serialize</param>
        /// <returns>Bytes array with serialized <see cref="Message"/></returns>
        public byte[] Serialize(Message message)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, message);
            return stream.ToArray();
        }
    }
}
