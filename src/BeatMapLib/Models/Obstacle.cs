using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Obstacle : TrackObjectBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ObstacleCustomData CustomData { get; set; } = new ObstacleCustomData(); // Obstacle custom data

        public double Duration  { get; set; } // Duration in beats

        [JsonConverter(typeof(IntEnumConverter))]
        public ObstacleType Type { get; set; } // Type of this obstacle

        public long Width { get; set; } // Width of this obstacle
    }
}