using System;
using System.Collections.Generic;
using System.Linq;
using Needle.Console.Core.Command;
using Needle.Console.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace Needle.Console.Core
{
    public class CommandProcessor
    {
        public static bool RunCommand(string entry, out string[] output)
        {
            string[] entries = entry.Split(' ');
            if (!CommandRegistry.Commands.ContainsKey(entries[0]))
            {
                output = new []{"Command not found"};
                return false;
            }
            List<ConsoleCommand> cmds = CommandRegistry.Commands.GetValueOrDefault(entries[0]);
            output = new string[cmds.Count];
            
            ConsoleCommand cmd = cmds[0];
            for (int i = 0; i < cmds.Count; cmd = cmds[i++])
            {
                Assert.IsNotNull(cmd, "cmd should not be null!");
                var argToParse = entries[0].Length + 1 <= entry.Length ? entry[(entries[0].Length + 1)..] : null;
                if (ParseParameters(argToParse, cmd.Parameters, out object[] outArgs, out string error))
                {
                    try
                    {
                        var outcome = cmd.Method.Invoke(cmd.Source, outArgs);
                        output[i] = cmd.Method.ReturnType == typeof(void) ? "Method was called!" : outcome.ToString();
                    }
                    catch (Exception ex)
                    {
                        output = new [] { ex.ToString() };
                        return false;
                    }
                }
                else
                {
                    output = new [] { error };
                    return false;
                }
            }
            return true;
        }

        // TO DO!!!
        private static bool ParseParameters(string entryArgs, Parameter[] parameters, out object[] result, out string error)
        {
            string[] args = GetArgs(entryArgs);
            List<object> outArgs = new List<object>();
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                if (args.Length <= i && !param.Required) outArgs.Add(param.Info.DefaultValue);
                else if (args.Length <= i)
                {
                    error = $"Expected at least  {i + 1} required parameters but got {args.Length}! Error while trying to parse arg {param.Name}!";
                    result = null;
                    return false;
                }
                else
                {
                    try
                    {
                        outArgs.Add(Convert.ChangeType(args[i], param.Info.ParameterType));
                    }
                    catch (Exception ex)
                    {
                        error =
                            $"Error while trying to parse arg {param.Name}:{param.Info.ParameterType}, provided value: \'{args[i]}\'. {ex.Message}";
                        result = null;
                        return false;
                    }
                }
            }
            result = outArgs.ToArray();
            error = "";
            return true;
        }

        private static string[] GetArgs(string rawArgs)
        {
            if (rawArgs == null) return Array.Empty<string>();
            List<string> args = rawArgs.Split(' ').ToList();
            int count = Utils.CountSubstringInString(rawArgs, "\"");
            // getting args in " 
            for (int i = 0; i < args.Count && count >= 2; i++)
            {
                if (!args[i].StartsWith("\"")) continue;
                string currentString = args[i];
                for (int j = i; j < args.Count; j++)
                { 
                    if (j > i) currentString += " " + args[j];
                    if (!args[j].EndsWith("\"")) continue;
                    for (int k = j; k > i; args.RemoveAt(k--));
                    args[i] = currentString.Substring(1, currentString.Length - 2);
                    count -= Utils.CountSubstringInString(currentString, "\""); // because " can be also inside, not in start/end
                    break;
                }
            }
            return args.ToArray();
        }
    }
}