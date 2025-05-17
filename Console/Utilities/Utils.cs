using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;

namespace NeedleAssets.Console.Utilities
{
    public static class Utils
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
        public static int GetNearestCharacterWithMaxDistance(TextMeshProUGUI textMeshProUGUI, Vector3 mousePosition, float maxDistance)
        {
            int nearestCharIndex = TMP_TextUtilities.FindNearestCharacter(textMeshProUGUI, mousePosition, null, false);
            if (nearestCharIndex == -1) return -1;
            // Get the world position of the character's center bounding box
            Vector3 charCenterBottom = (textMeshProUGUI.textInfo.characterInfo[nearestCharIndex].bottomLeft +
                                        textMeshProUGUI.textInfo.characterInfo[nearestCharIndex].topRight) / 2;
            Vector3 charWorldPosition = textMeshProUGUI.transform.TransformPoint(charCenterBottom);
            // Calculate the distance from the mouse position to the character
            float distance = Vector3.Distance(mousePosition, charWorldPosition);
            // If the distance is greater than maxDistance, return -1 (no valid character)
            if (distance > maxDistance) return -1;
            // Return the index of the nearest character within the max distance
            return nearestCharIndex;
        }
        
        public static int CountSubstringInString(string source, string substring) =>
            source.Length - source.Replace(substring, "").Length;
        
        public static bool IsConvertibleFromString(Type type)
        {
            var converter = TypeDescriptor.GetConverter(type);
            return converter.CanConvertFrom(typeof(string));
        }
        
        public static string[] GetArgs(string rawArgs)
        {
            if (rawArgs == null) return Array.Empty<string>();
            List<string> args = rawArgs.Split(' ').ToList();
            int count = Utils.CountSubstringInString(rawArgs, "\"");
            // getting args in " "
            for (int i = 0; i < args.Count && count >= 2; i++)
            {
                if (!args[i].StartsWith("\"")) continue;
                string currentString = args[i];
                for (int j = i; j < args.Count; j++)
                { 
                    if (j > i) currentString += " " + args[j];
                    if (!args[j].EndsWith("\"")) continue;
                    for (int k = j; k > i; args.RemoveAt(k--));
                    args[i] = currentString.Substring(1, currentString.Length - 2);
                    count -= Utils.CountSubstringInString(currentString, "\""); // because " can be also inside, not in start/end
                    break;
                }
            }
            return args.ToArray();
        }
    }
}