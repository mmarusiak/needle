using System;
using System.Collections.Generic;
using System.Linq;
using Needle.Console.Logger;
using Needle.Console.MethodsHandler;
using Needle.Console.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Needle.Console
{
    public class NeedleConsole : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI output;
        [SerializeField] private ScrollRect scrollRect;
        private List<string> _messages;
        private int _currentMessage;
        private static bool _isQuitting = false;
        
        private static NeedleConsole _instance;

        private void Awake() => _instance = this;
        private void Start() => ConsoleCommandRegistry.Initialize();

        public static void Log(params string[] message) => Log(MessageType.Debug, message);
        
        public static void Log(MessageType messageType, params string[] message) => Log( new Message(String.Join(" ", message), messageType));

        public static void Log(Message msg)
        {
            if (_instance != null) _instance.DisplayMessage(msg);
            else if (Application.isPlaying && !_isQuitting) Debug.LogError("You need to create NeedleConsole GameObject first! See Examples!");
        }

        public static void RegisterInstanceCommand(object source) =>
            ConsoleCommandRegistry.RegisterInstanceCommands(source);

        public static void UnregisterInstanceCommand(object source) =>
            ConsoleCommandRegistry.UnregisterInstanceCommands(source);

        private void DisplayMessage(Message msg)
        {
            string content = msg.Content;
            string color = NeedleColors.GetColor((int) msg.Type);
            
            if (msg.Type == MessageType.UserInput)  output.text += $"\n{Utils.AttributizeText( $"> {content}", "b", $"color={color}")}";
            else output.text += $"\n {Utils.AttributizeText( $"[{msg.Type.ToString()}]", $"color={color}", "b", "i")} {content}";
            
            RectTransform rectTransform = output.GetComponent<RectTransform>();
            
            if (rectTransform.sizeDelta.y >= output.preferredHeight) return;
            
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x,output.preferredHeight);
            scrollRect.verticalNormalizedPosition = 0f;
        }

        private static int CountSubstringInString(string source, string substring) =>
            source.Length - source.Replace(substring, "").Length;
        
        public void RunCommand(string entry)
        {
            string[] entries = entry.Split(" ");
            DisplayMessage(new Message(entry, MessageType.UserInput));
            List<string> args = entries.Length > 1 ? entries.Skip(1).ToList() : new List<string>();

            int count = CountSubstringInString(entry.Substring(entries[0].Length), "\"");
            // getting args in " 
            for (int i = 0; i < args.Count && count >= 2; i++)
            {
                if (!args[i].StartsWith("\"")) continue;
                string currentString = args[i];
                for (int j = i; j < args.Count; j++)
                { 
                    if (j > i) currentString += " " + args[j];
                    if (!args[j].EndsWith("\"")) continue;
                    for (int k = j; k > i; args.RemoveAt(k--));
                    args[i] = currentString.Substring(1, currentString.Length - 2);
                    count -= CountSubstringInString(currentString, "\""); // because " can be also inside, not in start/end
                    break;
                }
            }

            DisplayMessage(ConsoleCommandRegistry.Execute(entries[0], args.ToArray()));
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnPlayMode() => _isQuitting = false;
        
        private void OnApplicationQuit() => _isQuitting = true;
        
    }
}