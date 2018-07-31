using LegionContract;
using System.IO;

namespace LegionCore.Architecture
{
    public class LegionErrorDataOut: LegionDataOut
    {
        public LegionErrorDataOut(string message)
        {
            Message = message;
        }

        public string Message { get; set; }

        public override void SaveToStream(StreamWriter streamWriter)
        {
            streamWriter.WriteLine(Message);
        }
    }
}