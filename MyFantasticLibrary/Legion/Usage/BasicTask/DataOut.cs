using LegionContract;
using System.IO;

namespace BasicTask
{
    public class DataOut: LegionDataOut
    {
        public int Result { get; set; }

        public override void SaveToStream(StreamWriter streamWriter)
        {
            streamWriter.WriteLine(Result);
        }
    }
}
