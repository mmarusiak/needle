using System.Collections.Generic;
using NeedleAssets.Console.Core.Command;

namespace NeedleAssets.Console.UI.UserInput.Parameters
{
    public class NeedleParameterLogger : IParameterLogger
    {
        public string SuggestionText(ConsoleCommand command) => GetAllParameters(command.Parameters);
        
        private string GetAllParameters(Parameter[] parameters) => GetSubParameters("", parameters);
        
        private string GetSubParameters(string parentParameterName, Parameter[] subParameters)
        {
            List<string> result = new();
            foreach (var parameter in subParameters)
            {
                if (parameter.Generic) result.Add($"[{parameter.Info.ParameterType.Name}]{parentParameterName}:{parameter.Name}");
                else result.Add(GetSubParameters($"{parentParameterName}:{parameter.Name}", parameter.SubParameters));
            }
            return string.Join(", ", result);
        }
    }
}