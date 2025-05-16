using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeedleAssets.Console.UI.Entries
{
    public interface IEntryLogger<T> where T : Enum
    {
        public string EntryToLog(ConsoleLogEntry<T> logEntry, Dictionary<T, Color> typeToColor);
        public string[] PlayerTooltip(ConsoleLogEntry<T> logEntry, Dictionary<T, Color> typeToColor);
        public string[] DevTooltip(ConsoleLogEntry<T> logEntry, Dictionary<T, Color> typeToColor) => PlayerTooltip(logEntry, typeToColor);
    }
}