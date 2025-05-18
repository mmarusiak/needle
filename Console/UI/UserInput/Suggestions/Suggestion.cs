using NeedleAssets.Console.Core.Command;
using TMPro;

namespace NeedleAssets.Console.UI.UserInput.Suggestions
{
    public class Suggestion
    {
        private readonly TextMeshProUGUI _text;
        private ConsoleCommand _suggestedCommand;

        public Suggestion(TextMeshProUGUI text)
        {
            _text = text;
        }

        public void SetConsoleCommand(ConsoleCommand command, ISuggestionLogger suggestionLogger)
        {
            ShowText();
            _suggestedCommand = command;
            _text.text = suggestionLogger.SuggestionText(command);
        }
        
        public void HideText() => _text.gameObject.SetActive(false);
        public void ShowText() => _text.gameObject.SetActive(true);
    }
}