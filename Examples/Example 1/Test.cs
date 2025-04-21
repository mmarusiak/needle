using Needle.Console.MethodsHandler;
using UnityEngine;

namespace Needle.Examples.Example_1
{
    public class Test
    {
        [ConsoleMethod("testCommand", "Test", "Testing!", "arg1 is a test string", "arg2 is a test int")]
        public static void TestVoid(string arg1, int arg2)
        {
            Debug.Log("test" + arg1 + arg2);
        }
    }
}