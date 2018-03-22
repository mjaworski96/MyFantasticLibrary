using System.IO;

namespace LegionContract
{
    public abstract class LegionDataOut: IdentifiedById
    {
        public abstract void SaveToStream(StreamWriter streamWriter);
    }
}
