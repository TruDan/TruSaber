namespace BeatMapInfo
{
    public enum EventType : byte
    {
        ControlBackLasers          = 0,
        ControlRingLights          = 1,
        ControlLeftRotatingLasers  = 2,
        ControlRightRotatingLasers = 3,
        ControlCenterLights        = 4,
        ControlBoostLights         = 5,

        //Unused                     = 6,
        //Unused                     = 7,
        TriggerRingSpin      = 8,
        TriggerRingZoom      = 9,
        ChangeBeatsPerMinute = 10,

        //Unused                      = 11,
        ControlLeftRotatingLasersRotationSpeed  = 12,
        ControlRightRotatingLasersRotationSpeed = 13,
        EarlyRotation                           = 14,
        LateRotation                            = 15,
    }
}