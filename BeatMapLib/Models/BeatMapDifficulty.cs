using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    /// <summary>Beat Saber Beatmap Level</summary>
    public partial class BeatMapDifficulty
    {
        [JsonProperty("$schema", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Schema { get; set; }

        [JsonProperty("_customData", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomData { get; set; } // Difficulty custom data

        [JsonProperty("_events")]
        public Event[] Events { get; set; } // Environment and lighting events

        [JsonProperty("_notes")]
        public Note[] Notes { get; set; }

        [JsonProperty("_obstacles")]
        public Obstacle[] Obstacles { get; set; } // Walls

        [JsonProperty("_specialEventsKeywordFilters", NullValueHandling = NullValueHandling.Ignore)]
        public SpecialEventsKeywordFilters SpecialEventsKeywordFilters { get; set; }

        [JsonProperty("_version")]
        public string Version { get; set; }

        [JsonProperty("_waypoints", NullValueHandling = NullValueHandling.Ignore)]
        public Waypoint[] Waypoints { get; set; }
    }

    public partial class BeatMapDifficulty
    {
        public static BeatMapDifficulty FromJson(string json) =>
            JsonConvert.DeserializeObject<BeatMapDifficulty>(json, BeatMapInfoConverter.Settings);
    }
}