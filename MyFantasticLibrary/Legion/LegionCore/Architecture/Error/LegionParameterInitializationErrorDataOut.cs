using System;
using System.IO;

namespace LegionCore.Architecture.Error
{
    /// <summary>
    /// Error that occurs when input data are initialized
    /// </summary>
    public class LegionParameterInitializationErrorDataOut : LegionErrorDataOut
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exception">Exception that causes error</param>
        public LegionParameterInitializationErrorDataOut(Exception exception) : base(exception)
        {
        }
        /// <summary>
        /// Saves information about error to stream
        /// </summary>
        /// <param name="streamWriter">Writer to stream with results</param>
        public override void SaveToStream(StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"Parameter initialization error: {Exception.Message}");
        }
    }
}
