using System;

namespace RocketUI.Utilities.Helpers
{
    public static class MathHelpers
    {
        public static double PiOver180 = Math.PI / 180.0;
        
        public static int IntCeil(double value)
        {
            var i = (int) value;
            return value > i ? i + 1 : i;
        }
        public static double SinInterpolation(double a, double b, double t)
        {
            return a + Math.Sin(t * PiOver180) * (b - a);
        }
        
        public static int RoundToNearestInterval(int value, int interval)
        {	
            var scale = (1f / interval);
            return (int)(Math.Round(value * scale) / scale);
        }

        public static double RoundToNearestInterval(double value, double interval)
        {	
            var scale = (1f / interval);
            return Math.Round(value * scale) / scale;
        }

        public static float RoundToNearestInterval(float value, float interval)
        {	
            var scale = (1f / interval);
            return (float)(Math.Round(value * scale) / scale);
        }
    }
}