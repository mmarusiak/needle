using NeedleAssets.Console.Core.Command;

namespace NeedleAssets.Console.UI.UserInput.Suggestions
{
    public interface ISuggestionLogger
    {
        public string SuggestionText(ConsoleCommand command);
    }
}