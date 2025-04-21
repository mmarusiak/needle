using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Needle.Console.MethodsHandler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Needle.Console.Logger
{
    public class MessageLogger : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI output;
        private List<string> _messages;
        private int _currentMessage;
        private void DisplayMessage(Message msg)
        {
            string content = msg.Content;
            string color = NeedleColors.ColorToHex(NeedleColors.Colors[(int) msg.Type]);
            output.text += $"\n <b><i>[{msg.Type.ToString()}]</i></b> <color={color}>{content}</color>";
            // while(output.isTextOverflowing) output.text. handle wrapping text if it exceeds bounds - to do!
        }

        public void RunCommand(string entry)
        {
            string[] entries = entry.Split(" ");
            DisplayMessage(ConsoleCommandRegistry.Execute(entries[0], entries.Skip(1).ToArray()));
        }
    }
}