using System;

namespace Needle.Console.UI.Entries
{
    public interface IEntryLogger<T> where T : Enum
    {
        public string EntryToLog(ConsoleLogEntry<T> logEntry);
    }
}