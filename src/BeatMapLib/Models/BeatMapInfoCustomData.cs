using Newtonsoft.Json;

namespace BeatMapInfo
{
    /// <summary>Top-level custom data</summary>
    public partial class BeatMapInfoCustomData
    {
        [JsonProperty("_contributors", NullValueHandling = NullValueHandling.Ignore)]          public BeatmapContributors[] Contributors { get; set; }
        [JsonProperty("_customEnvironment", NullValueHandling = NullValueHandling.Ignore)]     public string CustomEnvironment { get; set; }          
        [JsonProperty("_customEnvironmentHash", NullValueHandling = NullValueHandling.Ignore)] public string CustomEnvironmentHash { get; set; }       // Used to match platforms on modelsaber.com
    }
}