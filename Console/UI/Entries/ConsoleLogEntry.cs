using System;

namespace Needle.Console.UI.Entries
{
    public class ConsoleLogEntry<T> where T : Enum
    {
        public T MessageType { get; }
        public string Content { get; }
        public string DevContent { get; }
        public DateTime Timestamp { get; }
        public object Source { get; }

        public ConsoleLogEntry(T messageType, string content, DateTime timestamp, object source, string devContent)
        {
            MessageType = messageType;
            Content = content;
            Timestamp = timestamp;
            Source = source;
            DevContent = devContent;
        }

        public string ToLog(IEntryLogger<T> logger) => logger.EntryToLog(this);
    }
}