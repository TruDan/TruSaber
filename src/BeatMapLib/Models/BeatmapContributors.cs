using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class BeatmapContributors
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IconPath { get; set; }

        public string Name { get; set; }
        public string Role { get; set; }
    }
}