using System;
using System.Collections.Generic;
using NeedleAssets.Console.Utilities;
using UnityEngine;

namespace NeedleAssets.Console.UI.Entries
{
    public class NeedleEntryLogger<T> : IEntryLogger<T> where T : Enum
    {
        public string EntryToLog(ConsoleLogEntry<T> logEntry, Dictionary<T, Color> typeToColor)
        {
            Color color = typeToColor.TryGetValue(logEntry.MessageType, out var value) ? value : Color.white;
            
            if (logEntry.IsInput) return Utils.ItalizeText(Utils.ColorizeText($"> {logEntry.Content}", color));
            return Utils.ItalizeText(Utils.ColorizeText($"[{logEntry.MessageType}]", color)) + $": {logEntry.Content}";
        }

        public string[] PlayerTooltip(ConsoleLogEntry<T> logEntry, Dictionary<T, Color> typeToColor)
        {
            return new [] {""};
        }

        public string[] DevTooltip(ConsoleLogEntry<T> logEntry, Dictionary<T, Color> typeToColor)
        {
            return new []
            {
                $"Content: {logEntry.Content}",
                $"Sent on: {logEntry.Timestamp}",
                $"Sent from: {logEntry.Source}, {logEntry.DevContent}",
                $"Additional call trace - to do:"
            };
        }
    }
}