using System;
using System.IO;

namespace LegionCore.Serialization
{
    public interface ISerial
    {
        void Serialize(object value, Type type, StreamWriter streamWriter);
        object Deserialize(Type type, StreamReader streamReader);
    }
}
