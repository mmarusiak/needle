using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Needle.Console.UI.Entries;
using TMPro;
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
        private TextMeshProUGUI _tooltip;

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

        public ConsoleUI (LogText output, IEntryLogger<T> entryLogger, Dictionary<T, Color> typeToColor, TextMeshProUGUI tooltip)
        {
            _typeToColor = typeToColor;
            _output = output;
            _entryLogger = entryLogger;
            _tooltip = tooltip;
            _output.AddHoverListener(DisplayTooltip);
            _output.AddQuitHoverListener(HideTooltip);
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
            _output.Text =  string.Join("\n", logs.Select(log => log.ToLog(_entryLogger, _typeToColor)));

        public void DisplayTooltip(int characterIndex)
        {
            UnityEngine.Debug.Log(characterIndex);

            int[] keys = _displayedLogs.Keys.ToArray();
            int target = keys[0];
            for (int i = 0; characterIndex > target; target = keys[++i]) ;
            var targetLog = _displayedLogs[target];
            
            _tooltip.gameObject.SetActive(true);
            // adjust pos of tooltip to mouse pos
            _tooltip.transform.position = Input.mousePosition - Vector3.up * _tooltip.rectTransform.sizeDelta.y / 1.5f;
#if UNITY_EDITOR
            _tooltip.text = String.Join("\n", _entryLogger.DevTooltip(targetLog, _typeToColor));
#else
            _tooltip.text = String.Join("\n", _entryLogger.PlayerTooltip(targetLog));
#endif
        }
        
        public void HideTooltip() => _tooltip.gameObject.SetActive(false);
        
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
            dictionary[DisplayedLogs.Count > 0 ? (DisplayedLogs.Keys.Last() + entry.ToLog(_entryLogger, _typeToColor).Length) : entry.ToLog(_entryLogger, _typeToColor).Length] = entry;
    }
}