using NeedleAssets.Console.Core.Command;

namespace NeedleAssets.Console.UI.UserInput.Suggestions
{
    public class NeedleSuggestionLogger : ISuggestionLogger
    {
        public string SuggestionText(ConsoleCommand command)
        {
            return $"{command.Name} {GetAllParameters(command.Parameters)}";
        }

        private string GetAllParameters(Parameter[] parameters)
        {
            string result = "";
            foreach (var parameter in parameters)
            {
                if (parameter.Generic) result += parameter.Name + " ";
                else result += GetSubParameters(parameter.Name, parameter.SubParameters);
            }
            return result;
        }
        
        private string GetSubParameters(string parentParameterName, Parameter[] subParameters)
        {
            string result = "";
            foreach (var parameter in subParameters)
            {
                if (parameter.Generic) result += parentParameterName + ":" + parameter.Name + " ";
                else result += GetSubParameters(parentParameterName+ ":" + parameter.Name, parameter.SubParameters);
            }
            return result;
        }
    }
}