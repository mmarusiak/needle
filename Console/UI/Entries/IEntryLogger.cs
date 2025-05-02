using System;
using System.Collections.Generic;
using UnityEngine;

namespace Needle.Console.UI.Entries
{
    public interface IEntryLogger<T> where T : Enum
    {
        public string EntryToLog(ConsoleLogEntry<T> logEntry, Dictionary<T, Color> typeToColor);
    }
}