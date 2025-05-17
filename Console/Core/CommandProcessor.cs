using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.Parser;
using NeedleAssets.Console.Utilities;
using UnityEngine;
using UnityEngine.Assertions;

namespace NeedleAssets.Console.Core
{
    public static class CommandProcessor
    {
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
            // static / runtime / gameobject
            Match objectExpression = Regex.Match(String.Join(' ', entries[1..]), @"^\[([^\]]+)\]");
            if (objectExpression.Success)
            {
                string[] names = Utils.GetArgs(objectExpression.Groups[1].Value.ToLower());
                bool statics = names.Contains("static");
                bool runtime = names.Contains("runtime");

                // remove commands without target source
                for (int j = cmds.Count - 1; j >= 0; j--)
                {
                    var c = cmds[j];
                    if (c.Source == null && !statics || c.Source != null && !runtime &&
                        !names.Contains((c.Source as MonoBehaviour)?.gameObject.name.ToLower()))
                        cmds.Remove(c);
                }
                paramsOffset += objectExpression.Groups[0].Value.Length + 1;
            }

            if (cmds.Count == 0)
            {
                output = new []{$"Command for {objectExpression.Groups[1].Value} not found"};
                return false;
            }
            
            // run commands
            output = new string[cmds.Count];
            int i = 0;
            foreach(var cmd in cmds)
            {
                Assert.IsNotNull(cmd, "cmd should not be null!");
                var argToParse = paramsOffset <= entry.Length ? entry[paramsOffset..] : null;
                ParameterParser parser = new ParameterParser(Utils.GetArgs(argToParse), cmd.Parameters);
                if (parser.Success)
                {
                    try
                    {
                        var outcome = cmd.Method.Invoke(cmd.Source, parser.Result);
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
                    output = new [] { parser.Error };
                    return false;
                }
            }
            return true;
        }
    }
}