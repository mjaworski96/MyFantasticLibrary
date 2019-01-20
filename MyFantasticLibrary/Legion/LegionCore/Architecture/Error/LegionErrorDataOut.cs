using LegionContract;
using System;
using System.IO;

namespace LegionCore.Architecture
{
    /// <summary>
    /// Error result representation
    /// </summary>
    public abstract class LegionErrorDataOut: LegionDataOut
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception">Exception that causes error</param>
        public LegionErrorDataOut(Exception exception)
        {
            Exception = exception;
        }
        /// <summary>
        /// Exception that causes error
        /// </summary>
        public Exception Exception { get; set; }
    }
}