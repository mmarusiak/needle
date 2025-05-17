using NeedleAssets.Console;
using NeedleAssets.Console.Core;
using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.Utilities;
using UnityEngine;

namespace NeedleAssets.Examples.Example_1
{
    public class Test : NeedleBehaviour
    {
        
        [ConsoleCommand("hello", "hello dear dev!")]
        [ParamDescriptor("some test description")]
        [ParamIdentifier("my first parameter")]
        public static string HelloWorld()
        {
            Debug.Log("Hello world!" );
            return "Success";
        }

        [ConsoleCommand("echo", "echo but static")]
        public static string StaticEcho(string text)
        {
            return $"static: {text}";
        }
        
        [ConsoleCommand("echo", "hello dear dev!")]
        [ParamDescriptor("some test description")]
        [ParamIdentifier("my first parameter")]
        public string Echo(string param)
        {
            return param;
        }
        

        [ConsoleCommand("test_vectors", "Test command for parsing vectors")]
        public string TestVectors(Vector2 vec2, Vector3 vec3)
        {
            return $"Got vec2: ({vec2.x}, {vec2.y}) and vec3: ({vec3.x}, {vec3.y}, {vec3.z})";
        }
        
        [ConsoleCommand("test_vector2", "Test command for parsing vectors")]
        public string TestVector2(Vector2 vec2)
        {
            return $"Got vec2: ({vec2.x}, {vec2.y})";
        }

        [ConsoleCommand("test_class", "Test command for parsing classes")]
        public string TestClass(TestClass testClass)
        {
            return $"testClass.Position = ({testClass.Position.x}, {testClass.Position.y}, {testClass.Position.z}), testClass.Name = {testClass.Name}, testClass.Age = {testClass.Age}";
        }
        

        public void Start()
        {
            Needle.Log("Hello World!");
            Needle.Log("Static Echo", MessageType.Debug);
        }
    }
}