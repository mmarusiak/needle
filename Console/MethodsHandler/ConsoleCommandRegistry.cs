#define ENABLE_HELP_COMMAND

using System;
using System.Collections.Generic;
using System.Reflection;
using Needle.Console.Logger;
using UnityEngine;

namespace Needle.Console.MethodsHandler
{
    public class ConsoleCommandRegistry : MonoBehaviour
    {
        private static readonly Dictionary<string, Command> Commands = new();
        // name of help command, can be custom command
        private const string HelpCommand = "help";

        private void Start()
        {
            RegisterConsoleCommands();
        }
        
        private static void RegisterConsoleCommands()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var attr = method.GetCustomAttribute<ConsoleMethod>();
                        if (attr != null)
                        {
                            CommandContainer container = attr.Container;
                            string commandName = container.Command;
                            Commands[commandName] = new Command(container, method);
                        }
                    }
                }
            }
        }

        private static object[] ParseArgs(string[] args, ParameterInfo[] parameterInfos)
        {
            List<object> outArgs = new ();
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                var param = parameterInfos[i];
                if (args.Length <= i && param.HasDefaultValue) outArgs.Add(param.DefaultValue);
                else if (args.Length <= i) return null;
                else outArgs.Add(Convert.ChangeType(args[i], param.ParameterType));
            }

            return outArgs.ToArray();
        }
        
        public static Message Execute(string commandName, string[] args)
        {
            if (!Commands.ContainsKey(commandName)) return new Message($"No command found! \n Type \'{HelpCommand}\' to get help with all commands!", MessageType.Error);
            
            Command cmd = Commands[commandName];
            ParameterInfo[] methodParams = cmd.Method.GetParameters();

            object[] outArgs = null;
            if (methodParams.Length != 0)
            {
                try
                {
                    outArgs = ParseArgs(args, methodParams);
                }
                catch (Exception e)
                {
                    return new Message(e.Message, MessageType.Error);
                }

                if (outArgs == null)
                    return new Message(
                        $"Expected {methodParams.Length}, but got {args.Length} arguments! \n Type \'{HelpCommand} {commandName}\' to get help with this command!",
                        MessageType.Error);
            }

            try
            {
                var result = cmd.Method.Invoke(null, outArgs);
                return new Message(cmd.Method.ReturnType == typeof(void) ? "Void was called!" : (string) result, MessageType.Info);
            }
            catch (Exception e)
            {
                return new Message((e.InnerException != null ? e.InnerException.Message : e.Message) + $"\n Type \'{HelpCommand} {cmd.Container.Command}\' to get help!", MessageType.Error);
            }
        }
#if ENABLE_HELP_COMMAND
        [ConsoleMethod(HelpCommand, "Help method", "Displays all commands with descriptions!")]
        public static string Help(string commandForHelp = null)
        {
            if (commandForHelp != null)
            {
                if (Commands.ContainsKey(commandForHelp)) return $"{Commands[commandForHelp].GetInfo()}";
                throw new Exception($"{commandForHelp} command not found!");
            }
            string r = "List of commands:";
            foreach (var cmd in Commands)
            {
                r += $"\n \n {cmd.Value.GetInfo()}";
            }
            return r;
        }
#endif
    }
}