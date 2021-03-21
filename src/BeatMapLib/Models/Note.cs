using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Note : TrackObjectBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public NoteCustomData CustomData { get; set; } = new NoteCustomData();// Note / bomb custom data

        [JsonConverter(typeof(IntEnumConverter))]
        public CutDirection CutDirection { get; set; }
        
        [JsonConverter(typeof(IntEnumConverter))]
        public NoteType Type { get; set; } // Type of this note
    }

    public class NoteCustomData : TrackObjectCustomData
    {
        public float? CutDirection { get; set; }
        public bool? DisableNoteGravity { get; set; }
    }
}