using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Needle.Console.MethodsHandler
{
    public class ConsoleCommandRegistry : MonoBehaviour
    {
        private static Dictionary<string, Command> commands = new();

        private void Start()
        {
            RegisterConsoleCommands();
            Debug.Log(Execute("help", null));
            Debug.Log(Execute("testCommand", null));
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
                            commands[commandName] = new Command(container, method);
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

        public static string Execute(string commandName, Object[] args)
        {
            if (!commands.ContainsKey(commandName)) return "No command found!";
            
            Command cmd = commands[commandName];

            int argsCount = args?.Length ?? 0;
            if (cmd.Method.GetParameters().Length > argsCount) 
                return $"Expected {cmd.Method.GetParameters().Length} parameters, got {argsCount} parameters!";

            Object result = cmd.Method.Invoke(null, args);
            return cmd.Method.ReturnType == typeof(void) ? "Void was called!" : (string) result;
        }

        [ConsoleMethod("help", "Help method", "Displays all commands with descriptions!")]
        public static string Help()
        {
            string r = "List of commands:";
            foreach (var cmd in commands)
            {
                r += $"\n \n {cmd.Value.GetInfo()}";
            }
            return r;
        }
    }
}