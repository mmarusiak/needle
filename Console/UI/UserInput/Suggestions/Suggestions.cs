using NeedleAssets.Console.Core;
using NeedleAssets.Console.Core.Registry;
using NeedleAssets.Console.UI.UserInput.Parameters;
using TMPro;
using UnityEngine;

namespace NeedleAssets.Console.UI.UserInput.Suggestions
{
    public class Suggestions : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI[] suggestionsTexts;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color selectionColor;
        
        private Suggestion[] _suggestions; 
        // default suggestion logger
        private IParameterLogger _parameterLogger = new NeedleParameterLogger();
        
        private void Awake()
        {
            _suggestions = System.Array.ConvertAll(suggestionsTexts, text => new Suggestion(text));
        }

        public void GetNewSuggestions(string entry)
        {
            // commands are stored here... we should first make tri tree and then update this tri tree each time new command is registered/unregistered
            // Needle.Log(CommandRegistry.Commands.Count);
        }
    }
}