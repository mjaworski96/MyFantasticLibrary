using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace AplicationInformationExchange.Model
{
    /// <summary>
    /// Parts of body
    /// </summary>
    [Serializable]
    public class BodyPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of page</param>
        public BodyPage(string name)
        {
            this.Name = name;

        }
        /// <summary>
        /// Name of page
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Body page content
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// String version of <see cref="Content"/>
        /// </summary>
        [JsonIgnore]
        public string StringContent { get => Encoding.UTF8.GetString(Content); }
        /// <summary>
        /// Converst <see cref="string"/> to byte array/>
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>Byte array created from given string</returns>
        private static byte[] ConvertStringToBytes(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        /// <summary>
        /// Creates body page from string
        /// </summary>
        /// <param name="name">Name of page</param>
        /// <param name="content">Body content</param>
        /// <returns>BodyPage created from given string</returns>
        public static BodyPage FromString(string name, string content)
        {
            return new BodyPage(name)
            {
                Content = ConvertStringToBytes(content)
            };
        }
        /// <summary>
        /// Creates page from given file
        /// </summary>
        /// <param name="filename">Path to file and page name</param>
        /// <returns>BodyPage created from file</returns>
        public static BodyPage FromFile(string filename)
        {
            return new BodyPage(filename)
            {
                Content = File.ReadAllBytes(filename)
            };
        }
        /// <summary>
        /// Creates page from object (serialized using JSON"/> )
        /// </summary>
        /// <param name="name">Name of page</param>
        /// <param name="obj">Object to create page</param>
        /// <returns>BodyPage created from given object</returns>
        public static BodyPage FromObject(string name, object obj)
        {
            return new BodyPage(name)
            {
                Content = ConvertStringToBytes(JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple
                }))
            };
        }
        /// <summary>
        /// Get object from content (JSON)
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns>Object from content</returns>
        public T ToObject<T>()
        {
            return JsonConvert.DeserializeObject<T>(StringContent, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Objects,
            });
        }
        /// <summary>
        /// Saves page to file
        /// </summary>
        public void ToFile()
        {
            File.WriteAllBytes(Name, Content);
        }
    }
}