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

        public void Start()
        {
            Needle.Log("Hello World!");
            Needle.Log("Static Echo", MessageType.Debug);
            Vector2 v2 = new Vector2();
        }
    }
}