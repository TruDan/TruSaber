using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    /// <summary>Beat Saber Beatmap Level</summary>
    public partial class BeatMapDifficulty
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomData { get; set; } // Difficulty custom data

        public Event[] Events { get; set; } // Environment and lighting events

        public Note[] Notes { get; set; }

        public Obstacle[] Obstacles { get; set; } // Walls

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SpecialEventsKeywordFilters SpecialEventsKeywordFilters { get; set; }

        public string Version { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Waypoint[] Waypoints { get; set; }
        
        [JsonIgnore]
        public Beatmap BeatMap { get; set; }
    }

    public partial class BeatMapDifficulty
    {
        public static BeatMapDifficulty FromJson(string json) => JsonConvert.DeserializeObject<BeatMapDifficulty>(json, BeatMapInfoConverter.Settings);
    }
}