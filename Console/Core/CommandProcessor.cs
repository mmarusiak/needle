using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace NeedleAssets.Console.Core
{
    public class CommandProcessor
    {
        // TO DO: 
        // here add [] object identifier
        public static bool RunCommand(string entry, out string[] output)
        {
            string[] entries = entry.Split(' ');
            int paramsOffset = entries[0].Length + 1;
            if (!CommandRegistry.Commands.ContainsKey(entries[0]) || CommandRegistry.Commands[entries[0]].Count == 0)
            {
                output = new []{"Command not found"};
                return false;
            }
            // bad looking clone...
            List<ConsoleCommand> cmds = CommandRegistry.Commands.GetValueOrDefault(entries[0]).ToArray<ConsoleCommand>().ToList();
            // [] object identifier
            Match objectExpression = Regex.Match(String.Join(' ', entries[1..]), @"^\[([^\]]+)\]");
            if (objectExpression.Success)
            {
                string[] names = GetArgs(objectExpression.Groups[1].Value.ToLower());
                bool statics = names.Contains("static");
                bool runtime = names.Contains("runtime");

                int j = 0;
                while (j < cmds.Count)
                {
                    var c = cmds[j];
                    if (c.Source == null && !statics || c.Source != null && !runtime &&
                        !names.Contains((c.Source as MonoBehaviour)?.gameObject.name.ToLower()))
                    {
                        cmds.Remove(c);
                        continue;
                    }

                    j++;
                }
                paramsOffset += objectExpression.Groups[0].Value.Length + 1;
            }

            if (cmds.Count == 0)
            {
                output = new []{$"Command for {objectExpression.Groups[1].Value} not found"};
                return false;
            }
            
            output = new string[cmds.Count];
            int i = 0;
            foreach(var cmd in cmds)
            {
                Assert.IsNotNull(cmd, "cmd should not be null!");
                var argToParse = paramsOffset <= entry.Length ? entry[paramsOffset..] : null;
                // check if it's generic parameter - it's constructor
                // move to another function I think...
                if (ParseParameters(argToParse, cmd.Parameters, out object[] outArgs, out string error))
                {
                    try
                    {
                        var outcome = cmd.Method.Invoke(cmd.Source, outArgs);
                        output[i++] = cmd.Method.ReturnType == typeof(void) ? "Method was called!" : outcome.ToString();
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
            // how to parse vectors/colors other types?
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
            // getting args in " "
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