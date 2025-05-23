using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.UI.UserInput.Parameters;
using NeedleAssets.Console.Utilities;
using TMPro;

namespace NeedleAssets.Console.UI.UserInput.Suggestions
{
    public class Suggestion
    {
        private readonly TextMeshProUGUI _text;
        private ConsoleCommand _suggestedCommand;
        private Suggestions _manager;
        public bool Hidden { get; private set; }
        public ConsoleCommand SuggestedCommand => _suggestedCommand;
        
        public Suggestion(TextMeshProUGUI text, Suggestions manager)
        {
            _text = text;
            Hidden = _text.gameObject.activeInHierarchy;
            _manager = manager;
        }

        public void SetConsoleCommand(ConsoleCommand command, string entry, IParameterLogger parameterLogger)
        {
            ShowText();
            _suggestedCommand = command;
            _text.text = $"{StyleByEntry(command, entry)} {string.Join(", ", parameterLogger.ParametersOverview(command))}";
        }

        public void NextParameter(int index, IParameterLogger parameterLogger)
        {
            ShowText();
            string[] parameters = parameterLogger.ParametersOverview(_suggestedCommand);
            for (int i = 0; i < index && i < parameters.Length; i++)
                parameters[i] = Utils.ColorizeText(Utils.BoldText(parameters[i]), _manager.SelectionColor);
            _text.text = $"{StyleByEntry(_suggestedCommand, _suggestedCommand.Name)} {string.Join(", ", parameters)}";
        }

        private string StyleByEntry(ConsoleCommand command, string entry) 
            => Utils.ColorizeText(Utils.BoldText(command.Name[..entry.Length]), _manager.SelectionColor) + command.Name[entry.Length..];

        public void HideText()
        {
            _text.gameObject.SetActive(false);
            Hidden = true;
        }

        public void ShowText()
        {
            _text.gameObject.SetActive(true);
            Hidden = false;
        }
    }
}