using System;
using System.IO;
using LegionContract;
using LegionCore.Architecture.Error;

namespace LegionCore.Architecture
{
    public class LegionErrorDataIn : LegionDataIn
    {
        public LegionErrorDataIn(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; set; }

        public override void LoadFromStream(StreamReader streamReader)
        {
            Exception = new Exception(streamReader.ReadLine());
        }
        public LegionParameterInitializationErrorDataOut TransformToDataOut()
        {
            return new LegionParameterInitializationErrorDataOut(Exception);
        }
    }
}
