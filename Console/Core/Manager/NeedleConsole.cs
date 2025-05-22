using System;
using System.Collections.Generic;
using NeedleAssets.Console.UI;
using NeedleAssets.Console.UI.Entries;
using UnityEngine;
using UnityEngine.UI;

namespace NeedleAssets.Console.Core.Manager
{
    public abstract class NeedleConsole<T> : NeedleConsoleBase where T : Enum
    {
        private static NeedleConsole<T> _instance;
        protected virtual Dictionary<T, Color> TypeToColors => new();
        protected abstract IEntryLogger<T> MessageLogger();
        private ConsoleUI<T> _console;
        
        protected virtual T Info => default(T);
        protected virtual T Warning => default(T);
        protected virtual T Error => default(T);
        protected virtual T Debug => default(T);
        protected virtual T Input => default(T);
        
        [SerializeField] protected LogText output;
        //[SerializeField] protected ConsoleTooltip tooltip;

        public override void Awake()
        {
            base.Awake();
            _console = new ConsoleUI<T>(output, MessageLogger(), TypeToColors, DeveloperMode(),Info,
                Warning,Error, Debug, Input);
            _instance = this;
        }

        public virtual void HandleInput(string input) => _console.HandleInput(input);
        
        public static void Log(object message) => _instance._console.Log(message.ToString());
        public static void LogDebug(object message) => _instance._console.Debug(message.ToString());
        public static void LogWarning(object message) => _instance._console.Warning(message.ToString());
        public static void LogError(object message) => _instance._console.Error(message.ToString());
        public static void LogColor(object message, Color color) =>
            _instance._console.Log(Utilities.Utils.ColorizeText(message.ToString(), color));
        public static void Log(object message, T type) => _instance._console.Log(message.ToString(), type);
        
        public static ConsoleLogEntry<T> GetTargetLog(int characterIndex) => _instance._console.GetTargetLog(characterIndex);
        
        public static string[] GetTooltipMessage(ConsoleLogEntry<T> logEntry)
        {
            if (_instance.DeveloperMode()) return _instance.MessageLogger().DevTooltip(logEntry, _instance.TypeToColors);
            return _instance.MessageLogger().PlayerTooltip(logEntry, _instance.TypeToColors);
        }
    }
}