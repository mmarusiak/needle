using UnityEngine;

namespace Needle.Console.Logger
{
    public class NeedleColors
    {
        public static readonly Color[] Colors =
        {
            new(255, 255, 255), // info
            new(225, 225, 55), // warning
            new(255, 0, 0), // error 
            new(200, 200, 200), // debug
        };
        
        public static string ColorToHex(Color color)
        {
            Color32 c32 = color;
            return $"#{c32.r:X2}{c32.g:X2}{c32.b:X2}";
        }
    }
}