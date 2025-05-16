
using Needle.Console.Core;
using Needle.Console.Core.Command;
using Needle.Console.Logger;
using UnityEngine;

namespace Needle.Examples.Example_1
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
            Console.Needle.Log("Hello World");
            Console.Needle.Log("Static Echo", MessageType.Debug);
        }
    }
}