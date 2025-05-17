using System;
using System.Collections.Generic;
using NeedleAssets.Console.UI;
using NeedleAssets.Console.UI.Entries;
using UnityEngine;

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
        
        [SerializeField] private LogText output;
        [SerializeField] private ConsoleTooltip tooltip;

        public override void Awake()
        {
            base.Awake();
            _console = new ConsoleUI<T>(output, MessageLogger(), TypeToColors, tooltip, DeveloperMode(),Info,
                Warning,Error, Debug, Input);
            _instance = this;
        }

        public virtual void HandleInput(string input) => _console.HandleInput(input);
        
        public static void Log(string message) => _instance._console.Log(message);
        
        public static void Log(string message, T type) => _instance._console.Log(message, type);
    }
}