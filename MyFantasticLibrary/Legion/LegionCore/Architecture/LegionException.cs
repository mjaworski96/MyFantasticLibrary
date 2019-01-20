using System;
using System.Runtime.Serialization;

namespace LegionCore.Architecture
{
    /// <summary>
    /// LegionException
    /// </summary>
    [Serializable]
    public class LegionException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LegionException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public LegionException(string message) : base(message)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Exception that causes current exception</param>
        public LegionException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">SerializationInfo</param>
        /// <param name="context">StreamingContext</param>
        protected LegionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}