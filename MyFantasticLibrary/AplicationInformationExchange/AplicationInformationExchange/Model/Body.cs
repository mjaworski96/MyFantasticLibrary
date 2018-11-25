using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AplicationInformationExchange.Model
{
    [Serializable]
    public class Body
    {
        [JsonProperty]
        private List<BodyPage> BodyContent { get; set; }
        [JsonIgnore]
        public int PagesCount { get => BodyContent.Count; }
        [JsonIgnore]
        public bool IsEmpty   { get => BodyContent.Count == 0; }
        [JsonIgnore]
        public IEnumerable<BodyPage> Pages   { get => BodyContent; }
        public Body() 
        {
            BodyContent = new List<BodyPage>();
        }
        public void Add(BodyPage bodyPage)
        {
            BodyContent.Add(bodyPage);
        }
        public BodyPage GetPage(int index)
        {
            if(index < 0 || index >= BodyContent.Count)
                return null;
            else
                return BodyContent[index];
        }
        public static Body Empty() 
        {
            return new Body();
        }

        public static Body FromFiles(IEnumerable<string> filenames)
        {
            Body body = new Body();
            foreach (var file in filenames)
            {
                body.Add(BodyPage.FromFile(file));
            }
            return body;
        }
        public static Body FromObject(string name, object obj)
        {
            Body body = new Body();
            body.Add(BodyPage.FromObject(name, obj));
            return body;
        }
        public void ToFiles()
        {
            foreach (var page in BodyContent)
            {
                page.ToFile();
            }
        }
    }
}