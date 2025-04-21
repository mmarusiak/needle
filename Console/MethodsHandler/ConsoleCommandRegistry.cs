using System;
using System.Collections.Generic;
using System.Reflection;
using Needle.Console.Logger;
using UnityEngine;
using Object = System.Object;

namespace Needle.Console.MethodsHandler
{
    public class ConsoleCommandRegistry : MonoBehaviour
    {
        private static readonly Dictionary<string, Command> Commands = new();

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
                            Debug.Log(method.ReturnParameter);
                            Debug.Log(method.GetParameters().Length);
                            foreach (var p in method.GetParameters())
                                Debug.Log(p.Name + p.ParameterType);
                            Debug.Log($"Registered console method: {commandName}");
                        }
                    }
                }
            }
        }

        public static Message Execute(string commandName, Object[] args)
        {
            if (!Commands.ContainsKey(commandName)) return new Message("No command found!", MessageType.Error);
            
            Command cmd = Commands[commandName];

            int argsCount = args?.Length ?? 0;
            if (cmd.Method.GetParameters().Length > argsCount) 
                return new Message($"Expected {cmd.Method.GetParameters().Length} parameters, got {argsCount} parameters!", MessageType.Error);

            try
            {
                Object result = cmd.Method.Invoke(null, args);
                return new Message(cmd.Method.ReturnType == typeof(void) ? "Void was called!" : (string) result, MessageType.Info);
            }
            catch (Exception e)
            {
                return new Message(e.Message + $"\n Type help {cmd.Container.Command} to get help!", MessageType.Error);
            }
        }

        [ConsoleMethod("help", "Help method", "Displays all commands with descriptions!")]
        public static string Help()
        {
            string r = "List of commands:";
            foreach (var cmd in Commands)
            {
                r += $"\n \n {cmd.Value.GetInfo()}";
            }
            return r;
        }
    }
}