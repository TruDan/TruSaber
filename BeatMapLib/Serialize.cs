using Newtonsoft.Json;

namespace BeatMapInfo
{
    public static partial class Serialize
    {
        public static string ToJson(this BeatMapInfo self) => JsonConvert.SerializeObject(self, BeatMapInfoConverter.Settings);
        public static string ToJson(this BeatMapDifficulty self) => JsonConvert.SerializeObject(self, BeatMapInfoConverter.Settings);
    }
}