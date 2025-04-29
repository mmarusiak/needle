using System;
using System.Collections.Generic;
using System.Linq;
using Needle.Console.Core.Command;
using UnityEngine.Assertions;

namespace Needle.Console.Core
{
    public class CommandProcessor
    {
        public static bool RunCommand(string entry, out string[] output)
        {
            string[] entries = entry.Split(' ');
            List<ConsoleCommand> cmds = CommandRegistry.Commands.GetValueOrDefault(entries[0]);
            output = new string[cmds.Count];
            
            ConsoleCommand cmd = cmds[0];
            for (int i = 0; i < cmds.Count; cmd = cmds[i++])
            {
                Assert.IsNotNull(cmd, "cmd should not be null!");
                if (ParseParameters(entries.Skip(1).ToArray(), cmd.Parameters, out object[] outArgs, out string error))
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

        public static bool ParseParameters(string[] args, Parameter[] parameters, out object[] result, out string error)
        {
            result = null;
            error = "";
            return true;
        }
    }
}