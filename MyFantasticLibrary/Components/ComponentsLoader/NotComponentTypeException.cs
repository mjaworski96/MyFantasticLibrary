using System;
using System.Runtime.Serialization;

namespace ComponentsLoader
{
    /// <summary>
    /// Exception for using type without ComponentAttribute
    /// </summary>
    [Serializable]
    public class NotComponentTypeException : Exception
    {
        /// <summary>
        /// Deafault constructor. Initializes new instance of NotComponentTypeException.
        /// </summary>
        public NotComponentTypeException()
        {
        }
        /// <summary>
        /// Initializes new instance of NotComponentTypeException.
        /// </summary>
        /// <param name="message">Message of exception.</param>
        public NotComponentTypeException(string message) : base(message)
        {
        }
        /// <summary>
        /// Initializes new instance of NotComponentTypeException.
        /// </summary>
        /// <param name="message">Message of exception.</param>
        /// <param name="innerException">Exception that caused current exception.</param>
        public NotComponentTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="info">Information about serialized object.</param>
        /// <param name="context">Context of serialization.</param>
        protected NotComponentTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}