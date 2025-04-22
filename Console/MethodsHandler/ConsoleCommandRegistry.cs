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

        public static Message Execute(string commandName, object[] args)
        {
            if (!Commands.ContainsKey(commandName)) return new Message($"No command found! \n Type \'{HelpCommand}\' to get help with all commands!", MessageType.Error);
            
            Command cmd = Commands[commandName];
            int argsCount = args?.Length ?? 0, optionalParams = 0;
            var methodParams = cmd.Method.GetParameters();
            
            for (int i = 0; i < methodParams.Length; optionalParams = methodParams[i++].HasDefaultValue ? optionalParams + 1 : optionalParams);
            if (methodParams.Length - optionalParams > argsCount || argsCount > methodParams.Length) 
                return new Message($"Expected {cmd.Method.GetParameters().Length} parameters, got {argsCount} parameters! " +
                                   $"\n Type \'{HelpCommand} {cmd.Container.Command}\' to get help!", MessageType.Error);
            
            if (methodParams.Length > argsCount)
            {
                List<object> allArgs = args == null ? new() : new(args);
                for (int i = allArgs.Count; i < methodParams.Length; i++)
                    if(methodParams[i].HasDefaultValue) allArgs.Add(methodParams[i].DefaultValue);
                args = allArgs.ToArray();
            }

            try
            {
                var result = cmd.Method.Invoke(null, args);
                return new Message(cmd.Method.ReturnType == typeof(void) ? "Void was called!" : (string) result, MessageType.Info);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e);
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
                throw new Exception("Command not found!");
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