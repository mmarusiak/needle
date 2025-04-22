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
        [SerializeField] private ScrollRect scrollRect;
        private List<string> _messages;
        private int _currentMessage;
        
        private void DisplayMessage(Message msg)
        {
            string content = msg.Content;
            string color = NeedleColors.ColorToHex(NeedleColors.Colors[(int) msg.Type]);
            
            if (msg.Type == MessageType.UserInput)  output.text += $"\n <b><i>>  <color={color}>{content}</color></i>";
            else output.text += $"\n <b><i>[{msg.Type.ToString()}]</i></b> <color={color}>{content}</color>";
            
            RectTransform rectTransform = output.GetComponent<RectTransform>();
            
            if (rectTransform.sizeDelta.y >= output.preferredHeight) return;
            
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,output.preferredHeight);
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void RunCommand(string entry)
        {
            string[] entries = entry.Split(" ");
            DisplayMessage(new Message(entry, MessageType.UserInput));
            DisplayMessage(ConsoleCommandRegistry.Execute(entries[0], entries.Skip(1).ToArray()));
        }
    }
}