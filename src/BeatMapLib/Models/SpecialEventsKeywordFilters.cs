using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class SpecialEventsKeywordFilters
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public Keyword[] Keywords { get; set; }
    }
}