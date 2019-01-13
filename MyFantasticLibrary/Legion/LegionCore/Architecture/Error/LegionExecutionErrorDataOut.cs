using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegionCore.Architecture.Error
{
    public class LegionExecutionErrorDataOut : LegionErrorDataOut
    {
        public LegionExecutionErrorDataOut(Exception exception) : base(exception)
        {
        }

        public override void SaveToStream(StreamWriter streamWriter)
        {
            streamWriter.WriteLine($"Task execution error: {Exception.Message}");
        }
    }
}
