using System;
using System.Collections.Generic;
using System.Reflection;
using Needle.Console.Core.Command;

namespace Needle.Console.Core
{
    public static class CommandRegistry
    {
        public static readonly Dictionary<string, List<ConsoleCommand>> Commands = new();

        // register parameters better!!! 
        
        public static void RegisterInstance(object instance)
        {
            Type type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var cmd = method.GetCustomAttribute<ConsoleCommand>();
                if (cmd == null) continue;
                            
                ParamIdentifier identifier = method.GetCustomAttribute<ParamIdentifier>();
                ParamDescriptor descriptor = method.GetCustomAttribute<ParamDescriptor>();
                            
                cmd.RegisterMethod(method, identifier, descriptor);
                cmd.RegisterSource(instance);
                            
                if (!Commands.ContainsKey(cmd.Name)) Commands[cmd.Name] = new List<ConsoleCommand>();
                Commands[cmd.Name].Add(cmd);
            }
        }

        public static void UnregisterInstance(object instance)
        {
            Type type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<ConsoleCommand>();
                if (attr == null) continue;
                
                List<ConsoleCommand> cmds = Commands[attr.Name];

                int i = 0;
                for (ConsoleCommand cmd = cmds[i]; i < cmds.Count && instance != cmd.Source; cmd = cmds[++i]);
                cmds.RemoveAt(i);
            }
        }

        public static void RegisterStaticCommands()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        var cmd = method.GetCustomAttribute<ConsoleCommand>();
                        if (cmd == null) continue;
                        
                        ParamIdentifier identifier = method.GetCustomAttribute<ParamIdentifier>(); 
                        ParamDescriptor descriptor = method.GetCustomAttribute<ParamDescriptor>();
                        
                        cmd.RegisterMethod(method, identifier, descriptor);
                        
                        if (!Commands.ContainsKey(cmd.Name)) Commands[cmd.Name] = new List<ConsoleCommand>(); 
                        Commands[cmd.Name].Add(cmd);
                    }
                }
            }
        }
    }
}