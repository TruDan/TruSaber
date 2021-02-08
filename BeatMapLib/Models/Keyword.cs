using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Keyword
    {
        [JsonProperty("_keyword")][JsonConverter(typeof(MinMaxLengthCheckConverter))] public string KeywordKeyword { get; set; }
        [JsonProperty("_specialEvents")]                                              public long[] SpecialEvents { get; set; } 
    }
}