using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NeedleAssets.Console.Core;
using NeedleAssets.Console.Core.Manager;
using NeedleAssets.Console.Core.Registry;
using NeedleAssets.Console.UI.Entries;
using NeedleAssets.Console.Utilities;
using UnityEngine;

namespace NeedleAssets.Console.UI
{
    public class ConsoleUI<T> where T : Enum
    {
        private readonly IEntryLogger<T> _entryLogger;
        
        private readonly Dictionary<T, Color> _typeToColor;
        private readonly LogText _output;

        private T[] _filters;
        
        // to do - associate types
        private readonly T _infoType;
        private readonly T _warningType;
        private readonly T _errorType;
        private readonly T _debugType;
        private readonly T _inputType;

        private readonly Dictionary<T, List<ConsoleLogEntry<T>>> _logs = new();
        private Dictionary<int, ConsoleLogEntry<T>> _displayedLogs = new ();
        
        public IEntryLogger<T> EntryLogger => _entryLogger;
        
        public Dictionary<int, ConsoleLogEntry<T>> DisplayedLogs
        {
            get => _displayedLogs;
            set
            {
                _displayedLogs = value;
                DisplayLogs(DisplayedLogs.Values.ToList());
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

        public ConsoleUI (LogText output, IEntryLogger<T> entryLogger, Dictionary<T, Color> typeToColor, bool developerMode,
            T infoType = default, T warningType = default, T errorType = default, T debugType = default, T inputType = default)
        {
            _typeToColor = typeToColor;
            _output = output;
            _entryLogger = entryLogger;
            // set default message types
            _infoType = infoType;
            _warningType = warningType;
            _errorType = errorType;
            _debugType = debugType;
            _inputType = inputType;
            CommandRegistry.RegisterStaticCommands();
        }

        public void Log(string message, T type, object source = null, [CallerMemberName] string memberName = "")
        {
            ConsoleLogEntry<T> logEntry = new ConsoleLogEntry<T>(type, message, DateTime.Now, source, memberName, type.Equals(_inputType));
            
            AddLog(logEntry);
            
            if (_filters != null && !_filters.Contains(type)) return;
            UpdateDictionaryLog(_displayedLogs, logEntry);
            DisplayLogs(_displayedLogs.Values.ToList());
        }

        private void AddLog(ConsoleLogEntry<T> logEntry)
        {
            if (!_logs.ContainsKey(logEntry.MessageType)) _logs.Add(logEntry.MessageType, new List<ConsoleLogEntry<T>>());
            _logs[logEntry.MessageType].Add(logEntry);
        }

        public void Log(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _infoType, source, memberName);
        public void Error(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _errorType, source, memberName);
        public void Warning(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _warningType, source, memberName);
        public void Debug(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _debugType, source, memberName);
        public void LogInput(string message, object source = null, [CallerMemberName] string memberName = "") => Log(message, _inputType, source, memberName);

        protected virtual void DisplayLogs(List<ConsoleLogEntry<T>> logs)
        {
            _output.Text = string.Join("\n", logs.Select(log => log.ToLog(_entryLogger, _typeToColor)));
            NeedleConsoleBase.OnOutputChanged();
        }

        public void HandleInput(string input)
        {
            if (Utils.CountSubstringInString(input, " ") == input.Length) return;
            LogInput(input);
            T commandType = CommandProcessor.RunCommand(input, out string[] output) ? _infoType : _errorType;
            foreach (string outmsg in output) Log(outmsg, commandType, this);
        }
        
        public void FilterBy(T[] filters)
        {
            filters = filters.Where(filter => _logs.ContainsKey(filter)).ToArray();
            _filters = filters;
            
            Dictionary<int, ConsoleLogEntry<T>> filtered = new();
            int[] i = new int[filters.Length];
            
            if (filters.Length == 0)
            {
                DisplayedLogs = filtered;
                return;
            }
            
            // it will be -1 if we hit list length
            while (i.Max() != -1)
            {
                UnityEngine.Debug.Log(String.Join(", ", i));
                ConsoleLogEntry<T> earliestLog = null;
                int earliestIndex = -1;
                for (int j = 0; j < i.Length; j++)
                {
                    if (i[j] == -1) continue;
                    var typeLog = _logs[filters[j]][i[j]];
                    if (earliestLog == null || earliestLog.Timestamp.CompareTo(typeLog) > 0)
                    {
                        earliestLog = typeLog;
                        earliestIndex = j;
                    }
                }
                
                i[earliestIndex]++; 
                if (_logs[filters[earliestIndex]].Count <= i[earliestIndex]) i[earliestIndex] = -1;
                
                UpdateDictionaryLog(filtered, earliestLog);
            }
            
            DisplayedLogs = filtered;
        }

        private void UpdateDictionaryLog(Dictionary<int, ConsoleLogEntry<T>> dictionary, ConsoleLogEntry<T> entry) => 
            dictionary[dictionary.Count > 0 ? (dictionary.Keys.Last() + entry.ToLog(_entryLogger, _typeToColor).Length) : entry.ToLog(_entryLogger, _typeToColor).Length] = entry;

        public ConsoleLogEntry<T> GetTargetLog(int characterIndex)
        {
            int[] keys = _displayedLogs.Keys.ToArray();
            if (keys.Length == 0) return null;
            
            int target = keys[0];
            for (int i = 1; i < keys.Length && characterIndex > target; target = keys[i++]) ;
            return _displayedLogs[target];
        }
    }
}