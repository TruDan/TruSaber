using Newtonsoft.Json;

namespace BeatMapInfo
{
    /// <summary>Custom data scoped to a single difficulty</summary>
    public partial class DifficultyBeatmapCustomData
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RgbColor ColorLeft { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RgbColor ColorRight { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DifficultyLabel { get; set; } // Custom label for this difficulty

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RgbColor EnvColorLeft { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RgbColor EnvColorRight { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Information { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public RgbColor ObstacleColor { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Requirements { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Suggestions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Warnings { get; set; }
    }
}