using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Obstacle : TrackObjectBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomData { get; set; } // Obstacle custom data

        public double Duration  { get; set; } // Duration in beats

        [JsonConverter(typeof(IntEnumConverter))]
        public ObstacleType Type { get; set; } // Type of this obstacle

        public byte Width { get; set; } // Width of this obstacle
    }
}