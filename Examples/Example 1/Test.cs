using Needle.Console.MethodsHandler;
using UnityEngine;

namespace Needle.Examples.Example_1
{
    public class Test
    {
        [ConsoleMethod("Test", "Testing!")]
        public static void TestVoid(string arg1, int arg2)
        {
            Debug.Log("test" + arg1 + arg2);
        }
    }
}