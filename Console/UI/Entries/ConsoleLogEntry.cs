using System;
using System.Collections.Generic;
using UnityEngine;

namespace NeedleAssets.Console.UI.Entries
{
    public class ConsoleLogEntry<T> where T : Enum
    {
        public T MessageType { get; }
        public string Content { get; }
        public string DevContent { get; }
        public DateTime Timestamp { get; }
        public object Source { get; }

        public bool IsInput { get; }

        public ConsoleLogEntry(T messageType, string content, DateTime timestamp, object source, string devContent, bool isInput = false)
        {
            MessageType = messageType;
            Content = content;
            Timestamp = timestamp;
            Source = source;
            DevContent = devContent;
            IsInput = isInput;
        }

        public string ToLog(IEntryLogger<T> logger, Dictionary<T, Color> typeToColor) => logger.EntryToLog(this, typeToColor);
    }
}