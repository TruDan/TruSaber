using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;

namespace BeatMapInfo
{
    public class TrackObjectCustomData
    {
        public Vector3? Position                { get; set; }
        public Vector3? Rotation                { get; set; }
        public Vector3? LocalRotation           { get; set; }
        public float?   NoteJumpMovementSpeed   { get; set; }
        public float?   NoteJumpStartBeatOffset { get; set; }
        public bool?    Fake                    { get; set; }
        public bool?    Interactable            { get; set; }
        
        [JsonExtensionData]
        public Dictionary<string, object> CustomData { get; set; } // Obstacle custom data
    }
}