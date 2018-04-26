using LegionContract;
using System.IO;

namespace MulAndWaitTask
{
    public class DataIn : LegionDataIn
    {
        public int A { get; set; }
        public int B { get; set; }
        public int C { get; set; }

        public override void LoadFromStream(StreamReader streamReader)
        {
            string[] data = streamReader.ReadLine().Split(',');
            A = int.Parse(data[0]);
            B = int.Parse(data[1]);
            C = int.Parse(data[2]);
        }
    }
}
