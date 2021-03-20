using System;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    /// <summary>Beat Saber Beatmap Info</summary>
    public partial class BeatMapInfo
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AllDirectionsEnvironmentName { get; set; }

        [JsonConverter(typeof(MinMaxValueCheckConverter))]
        public double BeatsPerMinute { get; set; }

        public string CoverImageFilename { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public BeatMapInfoCustomData CustomData { get; set; } // Top-level custom data

        public BeatmapSet[] DifficultyBeatmapSets { get; set; }
        public string       EnvironmentName       { get; set; }
        public string       LevelAuthorName       { get; set; }

        [JsonConverter(typeof(MinMaxValueCheckConverter))]
        public double PreviewDuration { get; set; } // Duration (in seconds) of level audio preview

        [JsonConverter(typeof(MinMaxValueCheckConverter))]
        public double
            PreviewStartTime { get; set; } // How long (in seconds) into beatmap audio the level preview will start

        [JsonConverter(typeof(MinMaxValueCheckConverter))]
        public double Shuffle { get; set; } // Time (in beats) of how much a note should shift when shuffled

        [JsonConverter(typeof(MinMaxValueCheckConverter))]
        public double ShufflePeriod { get; set; } // Time (in beats) of how often a note should shift

        public string SongAuthorName { get; set; } // Artist of this Beatmap's song
        public string SongFilename   { get; set; }
        public string SongName       { get; set; }
        public string SongSubName    { get; set; }
        public double SongTimeOffset { get; set; } // Offset between beatmap and audio (seconds)
        public string Version        { get; set; }
    }

    public partial class BeatMapInfo
    {
        public static BeatMapInfo FromJson(string json) =>
            JsonConvert.DeserializeObject<BeatMapInfo>(json, BeatMapInfoConverter.Settings);
    }
}