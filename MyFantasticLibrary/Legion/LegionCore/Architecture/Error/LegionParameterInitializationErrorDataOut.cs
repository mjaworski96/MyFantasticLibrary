using System;
using System.IO;

namespace LegionCore.Architecture.Error
{
    public class LegionParameterInitializationErrorDataOut : LegionErrorDataOut
    {
        public LegionParameterInitializationErrorDataOut(Exception exception) : base(exception)
        {
        }

        public override void SaveToStream(StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"Parameter initialization error: {Exception.Message}");
        }
    }
}
