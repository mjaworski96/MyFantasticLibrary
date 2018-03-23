using System;
using System.Runtime.Serialization;

namespace LegionCore.Architecture
{
    [Serializable]
    public class LegionException : Exception
    {
        public LegionException()
        {
        }

        public LegionException(string message) : base(message)
        {
        }

        public LegionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LegionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}