using System.Collections.Generic;
using Needle.Console.Logger;
using Needle.Console.UI;
using Needle.Console.UI.Entries;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Needle.Console
{
    public class Needle : MonoBehaviour
    {
        private Dictionary<MessageType, Color> _colors = new ();
        [SerializeField] private IEntryLogger<MessageType> _messageLogger = new NeedleEntryLogger<MessageType>();
        [SerializeField] private LogText output;
        [FormerlySerializedAs("_tooltip")] [SerializeField] private ConsoleTooltip tooltip;

        private ConsoleUI<MessageType> _console;
        private void Start()
        {
            _colors.Add (MessageType.Info, NeedleColors.Colors[0]);
            _colors.Add (MessageType.Warning, NeedleColors.Colors[1]);
            _colors.Add (MessageType.Error, NeedleColors.Colors[2]);
            _colors.Add (MessageType.Debug, NeedleColors.Colors[3]);
            _colors.Add (MessageType.UserInput, NeedleColors.Colors[4]);
            
            _console = new ConsoleUI<MessageType>(output, _messageLogger, _colors, tooltip, MessageType.Info, MessageType.Warning, MessageType.Error, MessageType.Debug, MessageType.UserInput);
            
            _console.Log("Welcome to the console!");
            _console.Warning("Warning");
            _console.Error("Error");
            _console.Debug("Debug");
        }
        
        public void HandleInput(string input) => _console.HandleInput(input);

        public void DummyLog()
        {
            _console.Log("Dummy log");
        }

        public void RandomFiler()
        {
            var filter = (MessageType) Random.Range(0, (int) MessageType.UserInput + 1);
            MessageType[] filters = {filter};
            _console.FilterBy(filters);
            _console.Log($"Random filter applied: {filters[0]}", filters[0]);
        }
    }
}