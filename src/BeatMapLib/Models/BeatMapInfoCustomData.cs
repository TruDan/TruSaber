using Newtonsoft.Json;

namespace BeatMapInfo
{
    /// <summary>Top-level custom data</summary>
    public partial class BeatMapInfoCustomData
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BeatmapContributors[] Contributors { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CustomEnvironment { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CustomEnvironmentHash { get; set; } // Used to match platforms on modelsaber.com
    }
}