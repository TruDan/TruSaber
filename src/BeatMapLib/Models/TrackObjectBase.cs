using Newtonsoft.Json;

namespace BeatMapInfo
{
    public class TrackObjectBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long LineLayer { get; set; } // Note horizontal position, starting at far left

        public long LineIndex { get; set; } // Note horizontal position, starting at far left

        public double Time { get; set; } // Time offset in beats
    }
}