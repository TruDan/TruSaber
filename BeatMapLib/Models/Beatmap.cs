using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Beatmap
    {
        [JsonProperty("_beatmapFilename")] 
        public string BeatmapFilename { get; set; }

        [JsonProperty("_customData", NullValueHandling = NullValueHandling.Ignore)]
        public DifficultyBeatmapCustomData CustomData { get; set; } // Custom data scoped to a single difficulty

        [JsonProperty("_difficulty")] 
        public Difficulty Difficulty { get; set; }
        
        [JsonProperty("_difficultyRank")] 
        public long DifficultyRank { get; set; }

        [JsonProperty("_noteJumpMovementSpeed")]
        public double NoteJumpMovementSpeed { get; set; } // Beatmap Note Jump Speed (NJS)

        [JsonProperty("_noteJumpStartBeatOffset")]
        public double NoteJumpStartBeatOffset { get; set; }
    }
}