using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Note : TrackObjectBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomData { get; set; } // Note / bomb custom data

        [JsonConverter(typeof(IntEnumConverter))]
        public CutDirection CutDirection { get; set; }
        
        [JsonConverter(typeof(IntEnumConverter))]
        public NoteType Type { get; set; } // Type of this note
    }
}