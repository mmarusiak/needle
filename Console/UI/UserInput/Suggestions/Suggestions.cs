using System;
using System.Collections.Generic;
using System.Linq;
using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.Core.Registry;
using NeedleAssets.Console.Core.Registry.TreeTri;
using NeedleAssets.Console.UI.UserInput.Parameters;
using NeedleAssets.Console.Utilities;
using TMPro;
using UnityEngine;

namespace NeedleAssets.Console.UI.UserInput.Suggestions
{
    public class Suggestions : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] suggestionsTexts;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color selectionColor;

        private TreeNode<ConsoleCommand> _suggestionParent = CommandRegistry.CommandTree.Root;
        
        private Suggestion[] _suggestions; 
        // default suggestion logger
        private readonly IParameterLogger _parameterLogger = new NeedleParameterLogger();

        private string _lastEntry = "";
        
        private void Awake()
        {
            _suggestions = System.Array.ConvertAll(suggestionsTexts, text => new Suggestion(text, this));
            foreach (var text in suggestionsTexts) text.color = normalColor;
        }

        public void GetNewSuggestions(string entry)
        {
            // to refactor all of this mess, entry does not need to change between old entry by 1 char, user can select delete f.e. ...
            // start of an arguments!
            if (entry.Contains(" "))
            {
                var suggs = _suggestions.Where(sug => !sug.Hidden).ToArray();
                if (suggs.Length != 1)
                {
                    foreach (var sugg in suggs) sugg.HideText();
                    return;
                }
                var sug = suggs[0];
                var e = entry.Split(' ')[0];
                if (sug.SuggestedCommand.Name != e)
                {
                    sug.HideText();
                    return;
                }
                sug.NextParameter(Utils.CountSubstringInString(entry," "), _parameterLogger);
                return;
            }

            if (_lastEntry == entry) return;

            var range = ..Utilities.Utils.Min(entry.Length, _lastEntry.Length);
            if (Math.Abs(entry.Length - _lastEntry.Length) == 1 && entry[range] == _lastEntry[range])
                _suggestionParent = GetNeighbour(entry.Length - _lastEntry.Length == 1, entry);
            else _suggestionParent = CommandRegistry.CommandTree.NodeByName(entry);
            
            //Needle.Log((entry[range] == _lastEntry[range]) + " - " + entry[range] + " - " + _lastEntry[range]);
            _lastEntry = entry;
            
            if (_suggestionParent == null)
            {
                SetStateOfAllSuggestions(false);
                return;
            }
            
            SetStateOfAllSuggestions(true);
            List<ConsoleCommand> commands = new List<ConsoleCommand>();
            AddSuggestions(_suggestionParent, commands, _suggestions.Length);
            int i = 0;
            for (; i < _suggestions.Length && i < commands.Count; i++) _suggestions[i].SetConsoleCommand(commands[i], entry, _parameterLogger);
            for (; i < _suggestions.Length; i++) _suggestions[i].HideText();
        }

        private TreeNode<ConsoleCommand> GetNeighbour(bool direction, string entry)
            => direction ? _suggestionParent?.Find(entry[^1]) : 
                _suggestionParent != null ? _suggestionParent.Parent : CommandRegistry.CommandTree.NodeByName(entry);
        

        private void AddSuggestions(TreeNode<ConsoleCommand> command, List<ConsoleCommand> suggestions, int amount)
        {
            foreach (var cmd in command.Values)
            {
                if (suggestions.Count >= amount) return;
                suggestions.Add(cmd);
            }

            foreach (var child in command.Children)
            {
                if (suggestions.Count >= amount) return;
                AddSuggestions(child, suggestions, amount);
            }
        }

        public void ResetSuggestions()
        {
            _suggestionParent = CommandRegistry.CommandTree.Root;
            SetStateOfAllSuggestions(true);
        }

        private void SetStateOfAllSuggestions(bool state)
        {
            foreach (var text in suggestionsTexts)
            {
                text.color = normalColor;
                text.gameObject.SetActive(state);
            }
        }
        
        public Color SelectionColor => selectionColor;
        public Color NormalColor => normalColor;
    }
}