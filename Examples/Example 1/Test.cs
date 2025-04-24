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
        
        [ConsoleMethod("testCommand2", "Test", "Testing!", "arg1 is a test string", "arg2 is a test string also")]
        public static void TestVoidB(string arg1, string arg2)
        {
            Debug.Log("test" + arg1 + " "+ arg2);
        }

        [ConsoleMethod("testHelp", "Test", "Testing!", "arg1 is a test string", "arg2 is a test string also")]
        public static string TestHelp(string arg1 = null)
        {
            if (arg1 == null) return "Arg is null but it's help, I'm helping you duh.";
            return $"{arg1}? Man, it's complicated, can't help you with that!";
        }
    }
}