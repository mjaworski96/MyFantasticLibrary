using System.IO;

namespace LegionContract
{
    /// <summary>
    /// Legion task input data
    /// </summary>
    public abstract class LegionDataIn: IdentifiedById
    {
        /// <summary>
        /// Loads input data from stream
        /// </summary>
        /// <param name="streamReader">Reader of stream with input data</param>
        public abstract void LoadFromStream(StreamReader streamReader);
    }
}
