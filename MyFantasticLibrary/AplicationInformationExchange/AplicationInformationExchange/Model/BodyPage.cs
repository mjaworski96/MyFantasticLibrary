using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace AplicationInformationExchange.Model
{
    [Serializable]
    public class BodyPage
    {
        public BodyPage(string name)
        {
            this.Name = name;

        }
        public string Name { get; set; }
        public byte[] Content { get; set; }

        [JsonIgnore]
        public string StringContent { get => Encoding.UTF8.GetString(Content); }

        private static byte[] ConvertStringToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        public static BodyPage FromString(string name, string content)
        {
            return new BodyPage(name)
            {
                Content = ConvertStringToBytes(content)
            };
        }
        public static BodyPage FromFile(string filename)
        {
            return new BodyPage(filename)
            {
                Content = File.ReadAllBytes(filename)
            };
        }
        public static BodyPage FromObject(string name, object obj)
        {
            return new BodyPage(name)
            {
                Content = ConvertStringToBytes(JsonConvert.SerializeObject(obj))
            };
        }
        public T ToObject<T>()
        {
            return JsonConvert.DeserializeObject<T>(StringContent);
        }
        public void ToFile()
        {
            File.WriteAllBytes(Name, Content);
        }
    }
}