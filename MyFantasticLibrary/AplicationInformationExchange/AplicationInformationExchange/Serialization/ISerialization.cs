using AplicationInformationExchange.Model;

namespace AplicationInformationExchange.Serialization
{
    /// <summary>
    /// Provides serialization
    /// </summary>
    public interface ISerialization
    {
        /// <summary>
        /// Deserialize message from byte array.
        /// </summary>
        /// <param name="content">Byte array to deserialize</param>
        /// <returns>Deserialized <see cref="Message"/></returns>
        Message Deserialize(byte[] content);
        /// <summary>
        /// Serialize message to byte array.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to serialize</param>
        /// <returns>Bytes array with serialized <see cref="Message"/></returns>
        byte[] Serialize(Message message);
    }
}