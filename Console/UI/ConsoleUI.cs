using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Needle.Console.UI.Entries;
using UnityEngine;

namespace Needle.Console.UI
{
    public class ConsoleUI<T> where T : Enum
    {
        private IEntryLogger<T> _entryLogger;
        
        private readonly List<ConsoleLogEntry<T>> _logs = new ();
        private Dictionary<int, ConsoleLogEntry<T>> _displayedLogs = new ();
        
        private Dictionary<T, Color> _typeToColor;
        private LogText _output;

        private T[] _filters;
        
        private T _infoType;
        private T _warningType;
        private T _errorType;
        private T _debugType;
        private T _inputType;
        
        public Dictionary<int, ConsoleLogEntry<T>> DisplayedLogs
        {
            get => _displayedLogs;
            set
            {
                _displayedLogs = value;
                DisplayLogs(DisplayedLogs.Values.ToList());
            }
        }

        public ConsoleUI (LogText output, IEntryLogger<T> entryLogger)
        {
            _output = output;
            _entryLogger = entryLogger;
            _output.AddListener(DisplayTooltip);
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
            
            if (_filters != null && !_filters.Contains(type)) return;
            UpdateDictionaryLog(_displayedLogs, logEntry);
            DisplayLogs(_displayedLogs.Values.ToList());
        }

        public void Log(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _infoType, source, memberName);
        public void Error(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _errorType, source, memberName);
        public void Warning(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _warningType, source, memberName);
        public void Debug(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _debugType, source, memberName);
        public void LogInput(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _inputType, source, memberName);

        protected virtual void DisplayLogs(List<ConsoleLogEntry<T>> logs) =>
            _output.Text =  string.Join("\n", logs.Select(log => log.ToLog(_entryLogger)));

        public void DisplayTooltip(int characterIndex)
        {
            UnityEngine.Debug.Log(characterIndex);

            int[] keys = _displayedLogs.Keys.ToArray();
            int target = keys[0];
            for (int i = 0; characterIndex > target; target = keys[++i]) ;
            
            UnityEngine.Debug.Log(characterIndex);
            UnityEngine.Debug.Log(target);
            UnityEngine.Debug.Log(_displayedLogs[target].Content);
        }
        
        
        public void FilterBy(T[] filters)
        {
            Dictionary<int, ConsoleLogEntry<T>> filtered = new();
            foreach (var log in _logs)
            {
                if (filters.Contains(log.MessageType)) UpdateDictionaryLog(filtered, log);
            }
            
            DisplayedLogs = filtered;
        }

        private void UpdateDictionaryLog(Dictionary<int, ConsoleLogEntry<T>> dictionary, ConsoleLogEntry<T> entry) => 
            dictionary[DisplayedLogs.Count > 0 ? (DisplayedLogs.Keys.Last() + entry.ToLog(_entryLogger).Length) : entry.ToLog(_entryLogger).Length] = entry;
    }
}