using LegionContract;
using System.IO;

namespace BasicTask
{
    public class DataOut: LegionData
    {
        public int Result { get; set; }

        public override void LoadFromStream(StreamReader streamReader)
        {
            Result = int.Parse(streamReader.ReadLine());
        }

        public override void SaveToStream(StreamWriter streamWriter)
        {
            streamWriter.WriteLine(Result);
        }
    }
}
