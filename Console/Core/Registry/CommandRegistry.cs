using System;
using System.Reflection;
using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.Core.Manager;
using NeedleAssets.Console.Core.Registry.TreeTri;
using UnityEngine;

namespace NeedleAssets.Console.Core.Registry
{
    public static class CommandRegistry
    {
        public static readonly CommandTree CommandTree = new();
        
        public static void RegisterInstance(object instance)
        {
            Type type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                var cmd = method.GetCustomAttribute<ConsoleCommand>();
                
                if (cmd == null || (cmd.DevCommand && !NeedleConsoleBase.InDeveloperMode)) continue;
                            
                ParamIdentifier identifier = method.GetCustomAttribute<ParamIdentifier>();
                ParamDescriptor descriptor = method.GetCustomAttribute<ParamDescriptor>();
                
                cmd.RegisterMethod(method, identifier, descriptor);
                cmd.RegisterSource(instance);
 
                CommandTree.AddNode(cmd);
            }
        }

        public static void UnregisterInstance(object instance)
        {
            Type type = instance.GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            foreach (var method in methods)
            {
                var cmd = method.GetCustomAttribute<ConsoleCommand>();
                if (cmd == null) continue;
                
                cmd.RegisterSource(instance);
                if(CommandTree.RemoveNode(cmd) ?? false) Debug.LogWarning($"Failed to unregister '{cmd.Name}', could not be found.");
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
                        
                        CommandTree.AddNode(cmd);
                    }
                }
            }
        }
    }
}