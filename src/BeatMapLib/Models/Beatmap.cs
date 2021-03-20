using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Beatmap
    {
        public string BeatmapFilename { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DifficultyBeatmapCustomData CustomData { get; set; } // Custom data scoped to a single difficulty

        public Difficulty Difficulty { get; set; }

        public long DifficultyRank { get; set; }

        public double NoteJumpMovementSpeed { get; set; } // Beatmap Note Jump Speed (NJS)

        public double NoteJumpStartBeatOffset { get; set; }
    }
}