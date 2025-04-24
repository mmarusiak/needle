using System;
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
            
            if (msg.Type == MessageType.UserInput)  output.text += $"\n <b><i><color={color}>> {content}</color></i>";
            else output.text += $"\n <color={color}><b><i>[{msg.Type.ToString()}]</i></b></color> {content}";
            
            RectTransform rectTransform = output.GetComponent<RectTransform>();
            
            if (rectTransform.sizeDelta.y >= output.preferredHeight) return;
            
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,output.preferredHeight);
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void RunCommand(string entry)
        {
            string[] entries = entry.Split(" ");
            DisplayMessage(new Message(entry, MessageType.UserInput));
            List<string> args = entries.Length > 1 ? entries.Skip(1).ToList() : new List<string>();
         
            // getting args in " 
            for (int i = 0; i < args.Count; i++)
            {
                if (!args[i].StartsWith("\"")) continue;
                string currentString = args[i];
                for (int j = i; j < args.Count; j++)
                { 
                    if (j > i) currentString += " " + args[j];
                    if (!args[j].EndsWith("\"")) continue;
                    for (int k = j; k > i; args.RemoveAt(k--));
                    args[i] = currentString.Substring(1, currentString.Length - 2);
                    break;
                }
            }

            DisplayMessage(ConsoleCommandRegistry.Execute(entries[0], args.ToArray()));
        }
    }
}