using System;
using System.Runtime.Serialization;

namespace ComponentsLoader
{
    [Serializable]
    public class NotComponentTypeException : Exception
    {
        public NotComponentTypeException()
        {
        }

        public NotComponentTypeException(string message) : base(message)
        {
        }

        public NotComponentTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotComponentTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}