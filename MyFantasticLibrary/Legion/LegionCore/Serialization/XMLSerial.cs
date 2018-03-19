using System;
using System.IO;
using System.Xml.Serialization;

namespace LegionCore.Serialization
{
    public class XmlSerial : ISerial
    {
        public object Deserialize(Type type, StreamReader streamReader)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            return xmlSerializer.Deserialize(streamReader);
        }

        public void Serialize(object value, Type type, StreamWriter streamWriter)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            xmlSerializer.Serialize(streamWriter, value);
        }
    }
}
