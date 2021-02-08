using Newtonsoft.Json;

namespace BeatMapInfo
{
    /// <summary>Custom data scoped to a single difficulty</summary>
    public partial class DifficultyBeatmapCustomData
    {
        [JsonProperty("_colorLeft", NullValueHandling = NullValueHandling.Ignore)]       public RgbColor ColorLeft { get; set; }    
        [JsonProperty("_colorRight", NullValueHandling = NullValueHandling.Ignore)]      public RgbColor ColorRight { get; set; }   
        [JsonProperty("_difficultyLabel", NullValueHandling = NullValueHandling.Ignore)] public string DifficultyLabel { get; set; } // Custom label for this difficulty
        [JsonProperty("_envColorLeft", NullValueHandling = NullValueHandling.Ignore)]    public RgbColor EnvColorLeft { get; set; } 
        [JsonProperty("_envColorRight", NullValueHandling = NullValueHandling.Ignore)]   public RgbColor EnvColorRight { get; set; }
        [JsonProperty("_information", NullValueHandling = NullValueHandling.Ignore)]     public string[] Information { get; set; }  
        [JsonProperty("_obstacleColor", NullValueHandling = NullValueHandling.Ignore)]   public RgbColor ObstacleColor { get; set; }
        [JsonProperty("_requirements", NullValueHandling = NullValueHandling.Ignore)]    public string[] Requirements { get; set; } 
        [JsonProperty("_suggestions", NullValueHandling = NullValueHandling.Ignore)]     public string[] Suggestions { get; set; }  
        [JsonProperty("_warnings", NullValueHandling = NullValueHandling.Ignore)]        public string[] Warnings { get; set; }     
    }
}