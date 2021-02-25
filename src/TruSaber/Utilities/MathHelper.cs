using System;
using BeatMapInfo;
using Microsoft.Xna.Framework;

namespace TruSaber
{
    public static class MathExtensions
    {
        public static float ToRadians(this float degrees)
        {
            return MathHelper.ToRadians(degrees);
        }
        
        public static float ToDegrees(this float radians)
        {
            return MathHelper.ToDegrees(radians);
        }
    }

    public static class CutDirectionExtensions
    {
        public static float ToAngle(this CutDirection direction)
        {
            switch (direction)
            {
                case CutDirection.Up:
                    return 0f.ToRadians();
                    break;
                case CutDirection.Down:
                    return 180f.ToRadians();
                    break;
                case CutDirection.Left:
                    return 90f.ToRadians();
                    break;
                case CutDirection.Right:
                    return 270f.ToRadians();
                    break;
                case CutDirection.UpLeft:
                    return 45f.ToRadians();
                    break;
                case CutDirection.UpRight:
                    return (270f + 45f).ToRadians();
                    break;
                case CutDirection.DownLeft:
                    return (180f - 45f).ToRadians();
                    break;
                case CutDirection.DownRight:
                    return (180f + 45f).ToRadians();
                    break;
                case CutDirection.Any:
                    return 0f.ToRadians();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}