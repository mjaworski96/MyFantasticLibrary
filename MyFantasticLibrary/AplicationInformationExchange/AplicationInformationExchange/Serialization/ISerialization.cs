using AplicationInformationExchange.Model;

namespace AplicationInformationExchange.Serialization
{
    public interface ISerialization
    {
        Message Deserialize(byte[] content);
        byte[] Serialize(Message message);
    }
}