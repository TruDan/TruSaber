using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public partial class Note
    {
        [JsonProperty("_customData", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> CustomData { get; set; } // Note / bomb custom data

        [JsonProperty("_cutDirection"), JsonConverter(typeof(IntEnumConverter))]
        public CutDirection CutDirection { get; set; }

        [JsonProperty("_lineIndex")]
        public byte LineIndex { get; set; } // Note horizontal position, starting at far left

        [JsonProperty("_lineLayer")]
        public byte LineLayer { get; set; } // Note vertical position, starting at bottom

        [JsonProperty("_time")]
        public double Time { get; set; } // Time offset in beats

        [JsonProperty("_type"), JsonConverter(typeof(IntEnumConverter))]
        public NoteType Type { get; set; } // Type of this note
    }

    public enum CutDirection
    {
        Up        = 0,
        Down      = 1,
        Left      = 2,
        Right     = 3,
        UpLeft    = 4,
        UpRight   = 5,
        DownLeft  = 6,
        DownRight = 7,
        Any       = 8
    }


    public enum NoteType
    {
        /// <summary>
        /// Red Note
        /// </summary>
        LeftNote = 0,
        
        /// <summary>
        /// Blue Note
        /// </summary>
        RightNote = 1,
        
        Unused = 2,
        
        Bomb = 3
    }
}