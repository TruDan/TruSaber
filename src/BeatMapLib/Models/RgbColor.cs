using Newtonsoft.Json;

namespace BeatMapInfo
{
    /// <summary>Color values are 0-1, not 0-255</summary>
    public partial class RgbColor
    {
        [JsonProperty("b")][JsonConverter(typeof(MinMaxValueCheckConverter))] public double B { get; set; }
        [JsonProperty("g")][JsonConverter(typeof(MinMaxValueCheckConverter))] public double G { get; set; }
        [JsonProperty("r")][JsonConverter(typeof(MinMaxValueCheckConverter))] public double R { get; set; }
    }
}