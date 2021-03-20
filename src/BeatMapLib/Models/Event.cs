using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Event
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomData { get; set; } // Environment / lighting event custom data

        public double Time { get; set; } // Time offset in beats

        [JsonConverter(typeof(IntEnumConverter))]
        public EventType Type { get; set; } // Type of this event

        public long Value { get; set; } // Parameter value for this event
    }
}