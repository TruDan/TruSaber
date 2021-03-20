using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Keyword
    {
        [JsonConverter(typeof(MinMaxLengthCheckConverter))]
        public string KeywordKeyword { get; set; }

        public long[] SpecialEvents { get; set; }
    }
}