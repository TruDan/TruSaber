using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Obstacle
    {
        [JsonProperty("_customData", NullValueHandling = NullValueHandling.Ignore)] public Dictionary<string, object> CustomData { get; set; } // Obstacle custom data
        [JsonProperty("_duration")]                                 public double Duration { get; set; }                       // Duration in beats
        [JsonProperty("_lineIndex")]                                public byte LineIndex { get; set; }                        // Obstacle horizontal position, starting at far left
        [JsonProperty("_time")]                                     public double Time { get; set; }                           // Time offset in beats
        [JsonProperty("_type")]                                     public byte Type { get; set; }                             // Type of this obstacle
        [JsonProperty("_width")]                                    public byte Width { get; set; }                            // Width of this obstacle
    }
}