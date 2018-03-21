using System.IO;

namespace LegionContract
{
    public abstract class LegionDataIn
    {
        public abstract void LoadFromStream(StreamReader streamReader);
    }
}
