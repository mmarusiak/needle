using Needle.Console.Logger;
using UnityEngine;

namespace Needle.Console.Utilities
{
    public class Utils
    {
        public static string ColorizeText(string text, Color color)
        {
            return AttributizeText(text, $"color={NeedleColors.ColorToHex(color)}");
        }

        public static string ItalizeText(string text) => AttributizeText(text, "i");
        
        public static string BoldText(string text) => AttributizeText(text, "b");
        
        public static string AttributizeText(string text, params string[] attributes)
        {
            foreach (string attr in attributes) text = $"<{attr}>{text}</{attr}>";
            return text;
        }
        
    }
}