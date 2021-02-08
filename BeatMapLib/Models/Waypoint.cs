using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Waypoint
    {
        [JsonProperty("_lineIndex")]       public long LineIndex { get; set; }       // Note horizontal position, starting at far left
        [JsonProperty("_lineLayer")]       public long LineLayer { get; set; }       // Note vertical position, starting at bottom
        [JsonProperty("_offsetDirection")] public long OffsetDirection { get; set; }
        [JsonProperty("_time")]            public double Time { get; set; }          // Duration in beats
    }
}