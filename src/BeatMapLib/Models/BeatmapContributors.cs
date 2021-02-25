using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class BeatmapContributors
    {
        [JsonProperty("_iconPath", NullValueHandling = NullValueHandling.Ignore)] public string IconPath { get; set; }
        [JsonProperty("_name")]                                   public string Name { get; set; }    
        [JsonProperty("_role")]                                   public string Role { get; set; }    
    }
}