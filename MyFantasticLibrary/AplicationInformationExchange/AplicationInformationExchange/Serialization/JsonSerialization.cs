using System.Text;
using AplicationInformationExchange.Model;
using Newtonsoft.Json;

namespace AplicationInformationExchange.Serialization
{
    public class JsonSerialization
    {
        public byte[] Serialize(Message message) 
        {
            string json = JsonConvert.SerializeObject(message);
            return Encoding.UTF8.GetBytes(json);
        }
        public Message Deserialize(byte[] content) 
        {
            string json = Encoding.UTF8.GetString(content);
            return JsonConvert.DeserializeObject<Message>(json);
        }
    }
}