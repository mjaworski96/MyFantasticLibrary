using System;
using Newtonsoft.Json;
using System.IO;

namespace LegionCore.Serialization
{
    public class JsonSerial : ISerial
    {
        private JsonSerializer Serializer
        {
            get
            {
                return new JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
            }
        }

        public void Serialize(object value, Type type, StreamWriter streamWriter)
        {
            JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter);
            Serializer.Serialize(jsonTextWriter, value, type);
            streamWriter.Flush();
        }
        public object Deserialize(Type type, StreamReader streamReader)
        {
            JsonTextReader jsonTextReader = new JsonTextReader(streamReader);
            return Serializer.Deserialize(jsonTextReader, type);
        }
    }
}
