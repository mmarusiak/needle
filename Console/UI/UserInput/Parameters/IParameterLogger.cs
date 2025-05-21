using NeedleAssets.Console.Core.Command;

namespace NeedleAssets.Console.UI.UserInput.Parameters
{
    public interface IParameterLogger
    {
        public string[] ParametersOverview(ConsoleCommand command);
        public string[] ParametersDescription(ConsoleCommand command);
    }
}