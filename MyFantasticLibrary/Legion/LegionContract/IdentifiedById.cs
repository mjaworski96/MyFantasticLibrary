using Newtonsoft.Json;

namespace LegionContract
{
    /// <summary>
    /// Base class for Task and Data(in/out)
    /// </summary>
    public abstract class IdentifiedById
    {
        /// <summary>
        /// Identifier of task or data (in/out)
        /// </summary>
        [JsonProperty]
        protected readonly int _Id = -1;
    }
}
