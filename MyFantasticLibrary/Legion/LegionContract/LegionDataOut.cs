using System.IO;

namespace LegionContract
{
    /// <summary>
    /// Legion task result
    /// </summary>
    public abstract class LegionDataOut: IdentifiedById
    {
        /// <summary>
        /// Save result to stream
        /// </summary>
        /// <param name="streamWriter">Writer to stream with results</param>
        public abstract void SaveToStream(StreamWriter streamWriter);
    }
}
