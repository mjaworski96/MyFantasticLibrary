using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegionCore.Architecture.Error
{
    /// <summary>
    /// Error that occurs when running task
    /// </summary>
    public class LegionExecutionErrorDataOut : LegionErrorDataOut
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception">Exception that causes error</param>
        public LegionExecutionErrorDataOut(Exception exception) : base(exception)
        {
        }
        /// <summary>
        /// Save information about error to stream
        /// </summary>
        /// <param name="streamWriter">Writer to stream with results</param>
        public override void SaveToStream(StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"Task execution error: {Exception.Message}");
        }
    }
}
