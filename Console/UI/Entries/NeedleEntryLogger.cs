using System;
using System.Collections.Generic;
using UnityEngine;

namespace Needle.Console.UI.Entries
{
    public class NeedleEntryLogger<T> : IEntryLogger<T> where T : Enum
    {
        public string EntryToLog(ConsoleLogEntry<T> logEntry, Dictionary<T, Color> typeToColor)
        {
            Color color = typeToColor.TryGetValue(logEntry.MessageType, out var value) ? value : Color.white;
            return Utilities.Utils.ColorizeText($"[{logEntry.MessageType}]", color) + $": {logEntry.Content}";
        }
    }
}