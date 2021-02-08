using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class BeatmapSet
    {
        [JsonProperty("_beatmapCharacteristicName")] public Characteristic BeatmapCharacteristicName { get; set; }
        [JsonProperty("_difficultyBeatmaps")]        public Beatmap[] DifficultyBeatmaps { get; set; }    
    }
}