using System;
using System.Runtime.Serialization;

namespace AplicationInformationExchange.Model
{
    /// <summary>
    /// Header of message
    /// </summary>
    [Serializable]
    public class Header
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="statusCode">Status of message</param>
        /// <param name="operationCode">Operation code</param>
        public Header(int statusCode, int operationCode)
        {
            StatusCode = statusCode;
            OperationCode = operationCode;
        }
        /// <summary>
        /// Status code of message
        /// </summary>
        public int StatusCode { get; private set; }
        /// <summary>
        /// Operation code of message.
        /// It specify what should be done with messaege.
        /// </summary>
        public int OperationCode { get; private set; }
    }
}