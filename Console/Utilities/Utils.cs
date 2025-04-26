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
    }
}