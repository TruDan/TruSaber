using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class SpecialEventsKeywordFilters
    {
        [JsonProperty("_keywords", NullValueHandling = NullValueHandling.Ignore)] public Keyword[] Keywords { get; set; }
    }
}