using System;
using System.IO;
using LegionContract;
using LegionCore.Architecture.Error;

namespace LegionCore.Architecture
{
    /// <summary>
    /// Error input data representation
    /// </summary>
    public class LegionErrorDataIn : LegionDataIn
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception">Exception that causes error</param>
        public LegionErrorDataIn(Exception exception)
        {
            Exception = exception;
        }
        /// <summary>
        /// Exception that causes error
        /// </summary>
        public Exception Exception { get; set; }
        /// <summary>
        /// Load exception message from stream.
        /// </summary>
        /// <param name="streamReader"></param>
        public override void LoadFromStream(StreamReader streamReader)
        {
            Exception = new Exception(streamReader.ReadLine());
        }
        /// <summary>
        /// Transform input data to error result
        /// </summary>
        /// <returns></returns>
        public LegionParameterInitializationErrorDataOut TransformToDataOut()
        {
            return new LegionParameterInitializationErrorDataOut(Exception);
        }
    }
}
