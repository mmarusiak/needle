using System;

namespace Needle.Console.UI.Entries
{
    public class NeedleEntryLogger<T> : IEntryLogger<T> where T : Enum
    {
        public string EntryToLog(ConsoleLogEntry<T> logEntry)
        {
            return logEntry.Content;
        }
    }
}