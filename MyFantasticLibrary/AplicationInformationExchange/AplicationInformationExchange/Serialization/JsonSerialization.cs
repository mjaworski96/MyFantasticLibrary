using System.Text;
using AplicationInformationExchange.Model;
using Newtonsoft.Json;

namespace AplicationInformationExchange.Serialization
{
    /// <summary>
    /// Serialization to JSON format
    /// </summary>
    public class JsonSerialization : ISerialization
    {
        /// <summary>
        /// Serialize message to byte array.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to serialize</param>
        /// <returns>Bytes array with serialized <see cref="Message"/></returns>
        public byte[] Serialize(Message message) 
        {
            string json = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(json);
        }
        /// <summary>
        /// Deserialize message from byte array.
        /// </summary>
        /// <param name="content">Byte array to deserialize</param>
        /// <returns>Deserialized <see cref="Message"/></returns>
        public Message Deserialize(byte[] content) 
        {
            string json = Encoding.UTF8.GetString(content);
            return JsonConvert.DeserializeObject<Message>(json);
        }
    }
}