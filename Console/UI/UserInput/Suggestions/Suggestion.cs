using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.UI.UserInput.Parameters;
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

        public void SetConsoleCommand(ConsoleCommand command, IParameterLogger parameterLogger)
        {
            ShowText();
            _suggestedCommand = command;
            _text.text = parameterLogger.SuggestionText(command);
        }
        
        public void HideText() => _text.gameObject.SetActive(false);
        public void ShowText() => _text.gameObject.SetActive(true);
    }
}