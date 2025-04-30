using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Needle.Console.UI.Entry;
using TMPro;

namespace Needle.Console.UI
{
    public abstract class ConsoleUI<T> where T : Enum
    {
        private List<ConsoleLogEntry<T>> _logs = new ();
        private List<ConsoleLogEntry<T>> _displayedLogs = new ();
        
        private TypeLinker<T> _linker;
        private TMP_Text _output, _input;

        private T[] _filters;
        
        private T _infoType;
        private T _warningType;
        private T _errorType;
        private T _debugType;
        private T _inputType;
        
        public List<ConsoleLogEntry<T>> DisplayedLogs
        {
            get => _displayedLogs;
            set
            {
                _displayedLogs = value;
                DisplayLogs(DisplayedLogs);
            }
        }

        public T[] Filters
        {
            get => _filters;
            set
            {
                _filters = value;
                FilterBy(_filters);
            }
        }

        public void Log(string message, T type, object source = null, [CallerMemberName] string memberName = "")
        {
            ConsoleLogEntry<T> logEntry = new ConsoleLogEntry<T>(type, message, DateTime.Now, source, memberName);
            
            _logs.Add(logEntry);
            
            if (!_filters.Contains(type)) return;
            _displayedLogs.Add(logEntry);
            DisplayLogs(_displayedLogs);
        }

        public void Log(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _infoType, source, memberName);
        public void Error(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _errorType, source, memberName);
        public void Warning(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _warningType, source, memberName);
        public void Debug(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _debugType, source, memberName);
        public void LogInput(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _inputType, source, memberName);

        public virtual void DisplayLogs(List<ConsoleLogEntry<T>> logs)
        {
        }
        
        public void FilterBy(T[] filters)
        {
            List<ConsoleLogEntry<T>> filtered = new();
            foreach (var log in _logs)
            {
                if (filters.Contains(log.MessageType)) filtered.Add(log);
            }
            
            DisplayedLogs = filtered;
        }
    }
}