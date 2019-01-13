using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AplicationInformationExchange.Model
{
    /// <summary>
    /// Message that can be sended
    /// </summary>
    [Serializable]
    public class Message
    {
        /// <summary>
        /// Header of message
        /// </summary>
        public Header Header { get; set; }
        /// <summary>
        /// Body of message
        /// </summary>
        public Body Body { get; set; }
        /// <summary>
        /// Creates message with empty body.
        /// </summary>
        /// <param name="statusCode">Status of message</param>
        /// <param name="operationCode">Operation code</param>
        /// <returns>Message with empty body</returns>
        public static Message WithEmptyBody(int statusCode, int operationCode)
        {
            return new Message()
            {
                Header = new Header(statusCode, operationCode),
                Body = Body.Empty()
            };
        }
        /// <summary>
        /// Creates message from string
        /// </summary>
        /// <param name="pageName">Name of body page</param>
        /// <param name="content">String content</param>
        /// <param name="statusCode">Status of message</param>
        /// <param name="operationCode">Operation code</param>
        /// <returns>Message with one page</returns>
        public static Message WithStringContent(string pageName, string content, int statusCode, int operationCode)
        {
            Body body = Body.Empty();
            body.Add(BodyPage.FromString(pageName, content));
            return new Message()
            {
                Header = new Header(statusCode, operationCode),
                Body = body
            };
        }
        /// <summary>
        /// Creates message from byte array.
        /// </summary>
        /// <param name="pageName">name of body page</param>
        /// <param name="content">Byte array content</param>
        /// <param name="statusCode">Status of message</param>
        /// <param name="operationCode">Operation code</param>
        /// <returns>Message with one page</returns>
        public static Message WithByteContent(string pageName, byte[] content, int statusCode, int operationCode)
        {
            Body body = Body.Empty();
            BodyPage bodyContent = new BodyPage(pageName);
            body.Add(bodyContent);
            bodyContent.Content = content;
            return new Message()
            {
                Header = new Header(statusCode, operationCode),
                Body = body
            };
        }
        /// <summary>
        /// Creates message from files
        /// </summary>
        /// <param name="filenames">Filenames to create message</param>
        /// <param name="statusCode">Status of message</param>
        /// <param name="operationCode">Operation code</param>
        /// <returns>Message with multiple pages (one page per file) </returns>
        public static Message FromFiles(IEnumerable<string> filenames, int statusCode, int operationCode)
        {
            Body body = Body.FromFiles(filenames);
            return new Message()
            {
                Header = new Header(statusCode, operationCode),
                Body = body
            };
        }
        /// <summary>
        /// Creates message from object
        /// </summary>
        /// <param name="name">Name of body page</param>
        /// <param name="obj">Object to create message</param>
        /// <param name="statusCode">Status of message</param>
        /// <param name="operationCode">Operation code</param>
        /// <returns>Message with one page</returns>
        public static Message FromObject(string name, object obj, int statusCode, int operationCode)
        {
            Body body = Body.FromObject(name, obj);
            return new Message()
            {
                Header = new Header(statusCode, operationCode),
                Body = body
            };
        }
        /// <summary>
        /// Add page to body
        /// </summary>
        /// <param name="page">Page to add</param>
        public void AddPage(BodyPage page) 
        {
            Body.Add(page);
        }
        /// <summary>
        /// Saves all body pages to files (one file per page).
        /// File names are same as body pages names.
        /// </summary>
        public void ToFiles()
        {
            Body.ToFiles();
        }
    }
}