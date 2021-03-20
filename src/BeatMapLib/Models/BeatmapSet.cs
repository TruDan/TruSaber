using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class BeatmapSet
    {
        public Characteristic BeatmapCharacteristicName { get; set; }
        public Beatmap[]      DifficultyBeatmaps        { get; set; }
    }
}