using System.IO;

namespace LegionContract
{
    public abstract class LegionDataOut
    {
        public abstract void SaveToStream(StreamWriter streamWriter);
    }
}
