using System;
using System.Linq;
using NeedleAssets.Console;
using NeedleAssets.Console.Core;
using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.Core.Registry;
using NeedleAssets.Console.UI.UserInput.Parameters;
using NeedleAssets.Console.UI.UserInput.Suggestions;
using UnityEngine;

namespace NeedleAssets.Examples.Example_1
{
    public class Test : NeedleBehaviour
    {
        
        [ConsoleCommand("hello", "hello dear dev!")]
        public static string HelloWorld()
        {
            Debug.Log("Hello world!" );
            return "Success";
        }
        
        
        [ConsoleCommand("echo", "hello dear dev!")]
        [ParamDescriptor("some test description")]
        [ParamIdentifier("my first parameter")]
        public string Echo(string param)
        {
            return param;
        }
        

        [ConsoleCommand("test_vectors", "Test command for parsing vectors")]
        [ParamDescriptor("Vector2 to test multiple not generic parameters", "Vector3 to test multiple not generic parameters")]
        public string TestVectors(Vector2 vec2, Vector3 vec3)
        {
            return $"Got vec2: ({vec2.x}, {vec2.y}) and vec3: ({vec3.x}, {vec3.y}, {vec3.z})";
        }
        
        [ConsoleCommand("test_vector2", "Test command for parsing vectors")]
        public string TestVector2(Vector2 vec2)
        {
            return $"Got vec2: ({vec2.x}, {vec2.y})";
        }

        [ConsoleCommand("test_class", "Test command for parsing classes", true)]
        public string TestClass(TestClass testClass)
        {
            return $"testClass.Position = ({testClass.Position.x}, {testClass.Position.y}, {testClass.Position.z}), testClass.Name = {testClass.Name}, testClass.Age = {testClass.Age}";
        }

        protected override void OnStart()
        {
            Needle.Log("Hello world!");
            Needle.LogColor("I'm red :o", Color.red);
        }

        [ConsoleCommand("help", "Helps with all commands")]
        public static string Help()
        {
            var cmds = CommandRegistry.CommandTree.AlphabeticalCommands();
            string[] keys = cmds.Keys.ToArray();
            string[] r = new string[keys.Length];
            for (int i = 0; i < keys.Length; i++)
            {
                // what with descriptions?
                var cmd = cmds[keys[i]][0];
                IParameterLogger logger = new NeedleParameterLogger();
                string[] parameters = logger.ParametersDescription(cmd);
                r[i] = parameters.Length > 0 ? $"Command: {keys[i]} \n\t{cmd.Description}\n\tDev command: {cmd.DevCommand}\n\tParameters: \n\t\t{string.Join("\n\t\t", parameters)}" : 
                    $"Command: {keys[i]} \n\t{cmd.Description}\n\tDev command: {cmd.DevCommand}";
            }

            return "List of all commands:\n" + string.Join("\n\n", r);
        }
    }
}