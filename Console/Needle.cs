using System.Collections.Generic;
using Needle.Console.Logger;
using Needle.Console.UI;
using Needle.Console.UI.Entries;
using UnityEngine;

namespace Needle.Console
{
    public class Needle : MonoBehaviour
    {
        private Dictionary<MessageType, Color> _colors = new Dictionary<MessageType, Color>();
        [SerializeField] private IEntryLogger<MessageType> _messageLogger = new NeedleEntryLogger<MessageType>();
        [SerializeField] private LogText output;

        private ConsoleUI<MessageType> _console;
        void Start()
        {
            _console = new ConsoleUI<MessageType>(output, _messageLogger);
            
            _console.Log("Welcome to the console!");
            _console.Log("some!");
            _console.Log("nvm!");
            _console.Log("Welcome t32131o the console!");
            _console.Log("Welcome 213t32o the console!");
            _console.Log("Welcome 21213to the console!");
            _console.Log("Welcome txso 12the console!");
            _console.Log("Welcome to 3213213312the console!");
        }
    }
}