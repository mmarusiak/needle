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
        private readonly Suggestions _manager;
        private int _index;
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
            _index = 0;
            ShowText();
            _suggestedCommand = command;
            _text.text = $"{StyleByEntry(command, entry)} {string.Join(", ", parameterLogger.ParametersOverview(command))}";
        }

        public void NextParameter(int index, IParameterLogger parameterLogger)
        {
            if (_suggestedCommand == null) return;
            ShowText();
            string[] parameters = parameterLogger.ParametersOverview(_suggestedCommand);
            for (int i = 0; i < index && i < parameters.Length; i++)
                parameters[i] = Utils.ColorizeText(Utils.BoldText(parameters[i]), _manager.HighlightedColor);
            _text.text = $"{StyleByEntry(_suggestedCommand, _suggestedCommand.Name)} {string.Join(", ", parameters)}";
            _index = index;
        }
        
        public void Redraw(IParameterLogger parameterLogger) => NextParameter(_index, parameterLogger);

        public ConsoleCommand SelectCommand(IParameterLogger parameterLogger)
        {
            _text.text = Utils.ColorizeText($"{_suggestedCommand.Name} {string.Join(", ", parameterLogger.ParametersOverview(_suggestedCommand))}", _manager.SelectionColor);
            return _suggestedCommand;
        }

        private string StyleByEntry(ConsoleCommand command, string entry) 
            => Utils.ColorizeText(Utils.BoldText(command.Name[..entry.Length]), _manager.HighlightedColor) + command.Name[entry.Length..];

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