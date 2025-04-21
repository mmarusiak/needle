using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = System.Object;

namespace Needle.Console.MethodsHandler
{
    public class ConsoleCommandRegistry : MonoBehaviour
    {
        private static Dictionary<string, MethodContainer> commands = new();

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
                    foreach (var method in type.GetMethods(BindingFlags.Static))
                    {
                        var attr = method.GetCustomAttribute<ConsoleMethod>();
                        if (attr != null)
                        {
                            string commandName = attr.GetName() ?? method.Name;
                            commands[commandName] = new MethodContainer(commandName, attr.GetDescription(), method);
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
            if (commands.ContainsKey(commandName))
            {
                commands[commandName].GetMethod().Invoke(null, args);
            }

            return "No command found!";
        }
    }
}