using System.IO;

namespace LegionContract
{
    public abstract class LegionDataIn: IdentifiedById
    {
        public abstract void LoadFromStream(StreamReader streamReader);
    }
}
