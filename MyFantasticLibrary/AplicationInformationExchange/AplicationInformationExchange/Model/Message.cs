using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace AplicationInformationExchange.Model
{
    public class Message
    {
        public Header Header { get; set; }
        public Body Body { get; set; }

        public static Message WithEmptyBody(int statusCode, int operationCode)
        {
            return new Message()
            {
                Header = new Header(statusCode, operationCode),
                Body = Body.Empty()
            };
        }
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
        public static Message FromFiles(IEnumerable<string> filenames, int statusCode, int operationCode)
        {
            Body body = Body.FromFiles(filenames);
            return new Message()
            {
                Header = new Header(statusCode, operationCode),
                Body = body
            };
        }
        public static Message FromObject(string name, object obj, int statusCode, int operationCode)
        {
            Body body = Body.FromObject(name, obj);
            return new Message()
            {
                Header = new Header(statusCode, operationCode),
                Body = body
            };
        }
        public void AddPage(BodyPage page) 
        {
            Body.Add(page);
        }
        public void ToFiles()
        {
            Body.ToFiles();
        }
    }
}