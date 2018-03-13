using System.IO;

namespace LegionContract
{
    public abstract class LegionData
    {
        public abstract void LoadFromStream(StreamReader streamReader);
        public abstract void SaveToStream(StreamWriter streamWriter);
    }
}
