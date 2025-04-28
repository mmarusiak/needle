using Needle.Console.Core.Command;
using UnityEngine;

namespace Needle.Examples.Example_1
{
    public class Test
    {
        [ConsoleCommand("hello", "hello dear dev!")]
        [ParamDescriptor("some test description")]
        [ParamIdentifier("my first parameter")]
        public static void HelloWorld(string param)
        {
            Debug.Log("Hello, " + param);
        }
    }
}