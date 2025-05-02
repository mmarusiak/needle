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
        [FormerlySerializedAs("_tooltip")] [SerializeField] private TextMeshProUGUI tooltip;

        private ConsoleUI<MessageType> _console;
        void Start()
        {
            _colors.Add (MessageType.Info, Color.green);
            _colors.Add (MessageType.Error, Color.red);
            _colors.Add (MessageType.Warning, Color.yellow);
            
            _console = new ConsoleUI<MessageType>(output, _messageLogger, _colors, tooltip);
            
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