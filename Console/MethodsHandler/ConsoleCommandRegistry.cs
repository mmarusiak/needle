#define ENABLE_HELP_COMMAND

using System;
using System.Collections.Generic;
using System.Reflection;
using Needle.Console.Logger;

namespace Needle.Console.MethodsHandler
{
    public static class ConsoleCommandRegistry
    {
        private static readonly Dictionary<string, List<Command>> Commands = new();
        // name of help command, can be custom command
        private const string HelpCommand = "help";

        public static void Initialize() => RegisterConsoleCommands();

        public static void RegisterInstanceCommands(object instance)
        {
            Type type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ConsoleMethod>();
                if (attr == null) continue;
                
                CommandContainer container = attr.Container;
                string commandName = container.Command;

                if (!Commands.ContainsKey(commandName)) Commands[commandName] = new List<Command>();
                Commands[commandName].Add(new Command(container, method, instance));
            }
        }

        public static void UnregisterInstanceCommands(object instance)
        {
            Type type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ConsoleMethod>();
                if (attr == null) continue;

                CommandContainer container = attr.Container;
                string commandName = container.Command;
                List<Command> cmds = Commands[commandName];
                
                for (int i = 0; i < cmds.Count; i++)
                {
                    if (cmds[i].TargetInstance != instance) continue;
                    cmds.RemoveAt(i);
                    break;
                }
            }
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
                            
                            if (!Commands.ContainsKey(commandName)) Commands[commandName] = new List<Command>();
                            Commands[commandName].Add(new Command(container, method));
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

        private static bool CallCommand(Command cmd, string commandName, string[] args, out string result)
        {
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
                    result = e.Message;
                    return false;
                }

                if (outArgs == null){
                    result = $"Expected {methodParams.Length}, but got {args.Length} arguments! \n Type \'{HelpCommand} {commandName}\' to get help with this command!";
                    return false;
                }
            }

            try
            {
                var commandOutcome = cmd.Method.Invoke(cmd.TargetInstance, outArgs);
                result = cmd.Method.ReturnType == typeof(void) ? "Void was called!" : (string) commandOutcome;
                return true;
            }
            catch (Exception e)
            {
                result = (e.InnerException != null ? e.InnerException.Message : e.Message) +
                         $"\n Type \'{HelpCommand} {cmd.Container.Command}\' to get help!";
                return false;
            }
        }
        
        public static Message Execute(string commandName, string[] args)
        {
            if (!Commands.ContainsKey(commandName) || Commands[commandName].Count == 0) return new Message($"Command \'{commandName}\' was not found! \n Type \'{HelpCommand}\' to get help with all commands!", MessageType.Error);

            string outcome = "";
            foreach (var cmd in Commands[commandName])
            {
                bool success = CallCommand(cmd, commandName, args, out string message);
                // if error occured
                string originPointer = $"[origin: {(cmd.TargetInstance == null ? "static method" : cmd.TargetInstance.ToString())}]";
                if (!success) return new Message(message + $"{originPointer}", MessageType.Error);
                outcome += message + $"{originPointer}, ";
            }

            return new Message(outcome, MessageType.Info);
        }
        
#if ENABLE_HELP_COMMAND
        [ConsoleMethod(HelpCommand, "Help method", "Displays all commands with descriptions!")]
        public static string Help(string commandForHelp = null)
        {
            if (commandForHelp != null)
            {
                if (Commands.ContainsKey(commandForHelp))
                {
                    if (Commands[commandForHelp].Count == 0) return $"Command {commandForHelp} unavailable.";
                    return $"{Commands[commandForHelp][0].GetInfo()}";
                }
                throw new Exception($"Command {commandForHelp} not found!");
            }
            string r = "List of commands:";
            foreach (var cmd in Commands)
            {
                if (cmd.Value.Count == 0) continue;
                r += $"\n \n {cmd.Value[0].GetInfo()}";
            }
            return r;
        }
#endif
    }
}