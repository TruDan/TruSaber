using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Obstacle
    {
        [JsonProperty("_customData", NullValueHandling = NullValueHandling.Ignore)] public Dictionary<string, object> CustomData { get; set; } // Obstacle custom data
        [JsonProperty("_duration")]                                 public double Duration { get; set; }                       // Duration in beats
        [JsonProperty("_lineIndex")]                                public long LineIndex { get; set; }                        // Obstacle horizontal position, starting at far left
        [JsonProperty("_time")]                                     public double Time { get; set; }                           // Time offset in beats
        [JsonProperty("_type")]                                     public long Type { get; set; }                             // Type of this obstacle
        [JsonProperty("_width")]                                    public long Width { get; set; }                            // Width of this obstacle
    }
}