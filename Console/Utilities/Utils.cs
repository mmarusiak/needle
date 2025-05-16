using System;
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

        public static int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
                return b?.Length ?? 0;
            if (string.IsNullOrEmpty(b))
                return a.Length;

            int[,] dp = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++)
                dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                    dp[i, j] = Mathf.Min(
                        dp[i - 1, j] + 1,      // Deletion
                        dp[i, j - 1] + 1,      // Insertion
                        dp[i - 1, j - 1] + cost // Replacement
                    );
                }
            }

            return dp[a.Length, b.Length];
        }
        
        public static bool TryGetSubstringCharacterRange(TextMeshProUGUI textMesh, string targetSubstring, out int startIndex, out int endIndex)
        {
            startIndex = -1;
            endIndex = -1;

            string fullText = textMesh.text;
            int substringStart = fullText.IndexOf(targetSubstring, StringComparison.Ordinal);

            if (substringStart == -1)
                return false;

            startIndex = substringStart;
            endIndex = startIndex + targetSubstring.Length - 1;
            return true;
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
    }
}