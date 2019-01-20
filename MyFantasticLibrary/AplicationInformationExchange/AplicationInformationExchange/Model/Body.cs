using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AplicationInformationExchange.Model
{
    /// <summary>
    /// Body of message
    /// </summary>
    [Serializable]
    public class Body
    {
        [JsonProperty]
        private List<BodyPage> BodyContent { get; set; }
        /// <summary>
        /// Pages count
        /// </summary>
        [JsonIgnore]
        public int PagesCount { get => BodyContent.Count; }
        /// <summary>
        /// True if body contains no pages
        /// </summary>
        [JsonIgnore]
        public bool IsEmpty   { get => BodyContent.Count == 0; }
        /// <summary>
        /// Pages of body
        /// </summary>
        [JsonIgnore]
        public IEnumerable<BodyPage> Pages   { get => BodyContent; }
        /// <summary>
        /// Default constructor
        /// </summary>
        public Body() 
        {
            BodyContent = new List<BodyPage>();
        }
        /// <summary>
        /// Add page to body
        /// </summary>
        /// <param name="bodyPage">Page to add</param>
        public void Add(BodyPage bodyPage)
        {
            BodyContent.Add(bodyPage);
        }
        /// <summary>
        /// Get page
        /// </summary>
        /// <param name="index">Index of page</param>
        /// <returns>Page with given index (if exists) or null (if index is out of range)</returns>
        public BodyPage GetPage(int index)
        {
            if(index < 0 || index >= BodyContent.Count)
                return null;
            else
                return BodyContent[index];
        }
        /// <summary>
        /// Creates empty body/
        /// </summary>
        /// <returns></returns>
        public static Body Empty() 
        {
            return new Body();
        }
        /// <summary>
        /// Creates body from files
        /// </summary>
        /// <param name="filenames">Filenames to create body</param>
        /// <returns>Body with multiple pages (one page per file)</returns>
        public static Body FromFiles(IEnumerable<string> filenames)
        {
            Body body = new Body();
            foreach (var file in filenames)
            {
                body.Add(BodyPage.FromFile(file));
            }
            return body;
        }
        /// <summary>
        /// Creates body from object
        /// </summary>
        /// <param name="name">Name of page</param>
        /// <param name="obj">Object to create body</param>
        /// <returns>Body with one page</returns>
        public static Body FromObject(string name, object obj)
        {
            Body body = new Body();
            body.Add(BodyPage.FromObject(name, obj));
            return body;
        }
        /// <summary>
        /// Saves body to files.
        /// </summary>
        public void ToFiles()
        {
            foreach (var page in BodyContent)
            {
                page.ToFile();
            }
        }
    }
}