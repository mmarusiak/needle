using System.Collections.Generic;
using NeedleAssets.Console.Core.Command;

namespace NeedleAssets.Console.UI.UserInput.Parameters
{
    public class NeedleParameterLogger : IParameterLogger
    {
        public string[] ParametersOverview(ConsoleCommand command) => GetAllParameters(command.Parameters);

        public string[] ParametersDescription(ConsoleCommand command)
        {
            string[] overview = GetAllParameters(command.Parameters);
            for (int i = 0; i < overview.Length; i++)
            {
                if (command.Parameters[i].Description.Length > 0) overview[i] = $"{command.Parameters[i].Description}: {overview[i]}";
                else overview[i] = $"{overview[i]}";
            }
            return overview;
        }
        
        private string[] GetAllParameters(Parameter[] parameters) => GetSubParameters("", parameters);
        
        private string[] GetSubParameters(string parentParameterName, Parameter[] subParameters)
        {
            List<string> result = new();
            foreach (var parameter in subParameters)
            {
                if (parameter.Generic) result.Add($"[{parameter.Info.ParameterType.Name}]{parentParameterName}:{parameter.Name}");
                else result.Add(string.Join(", ", GetSubParameters($"{parentParameterName}:{parameter.Name}", parameter.SubParameters)));
            }
            return result.ToArray();
        }
    }
}