using LegionContract;
using System;
using System.IO;

namespace LegionCore.Architecture
{
    public abstract class LegionErrorDataOut: LegionDataOut
    {
        public LegionErrorDataOut(Exception exception)
        {
            Exception = exception;
        }
        public Exception Exception { get; set; }
    }
}