using System.IO;
using LegionContract;

namespace BasicTask
{
    public class DataIn: LegionData
    {
        public int A { get; set; }
        public int B { get; set; }
        public int Wait { get; set; }

        public override void LoadFromStream(StreamReader streamReader)
        {
            string[] data = streamReader.ReadLine().Split(',');
            A = int.Parse(data[0]);
            B = int.Parse(data[1]);
            Wait = int.Parse(data[2]);
        }

        public override void SaveToStream(StreamWriter streamWriter)
        {
            streamWriter.WriteLine(A + "," + B + "," + Wait);
        }
    }
}
