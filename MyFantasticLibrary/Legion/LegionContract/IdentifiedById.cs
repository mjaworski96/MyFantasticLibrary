using Newtonsoft.Json;

namespace LegionContract
{
    public abstract class IdentifiedById
    {
        [JsonProperty]
        protected readonly int _Id = -1;
    }
}
