using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Event
    {
        [JsonProperty("_customData", NullValueHandling = NullValueHandling.Ignore)] public Dictionary<string, object> CustomData { get; set; } // Environment / lighting event custom data
        [JsonProperty("_time")]                                     public double Time { get; set; }                           // Time offset in beats
        [JsonProperty("_type")]                                     public long Type { get; set; }                             // Type of this event
        [JsonProperty("_value")]                                    public long Value { get; set; }                            // Parameter value for this event
    }
}