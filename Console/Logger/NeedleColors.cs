using UnityEngine;

namespace Needle.Console.Logger
{
    public class NeedleColors
    {
        public static readonly Color[] Colors =
        {
            new(0, .6f, 0), // info
            new(.9f, .9f, 0.22f), // warning
            new(1, g: 0, 0), // error 
            new(.40f, .40f, .40f), // debug
            new(.69f, .69f, .69f) // user input
        };

        public static string GetColor(int index) => ColorToHex(Colors[index]);
        
        public static string ColorToHex(Color color)
        {
            Color32 c32 = color;
            return $"#{c32.r:X2}{c32.g:X2}{c32.b:X2}";
        }
    }
}