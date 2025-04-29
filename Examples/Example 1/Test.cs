using Needle.Console.Core;
using Needle.Console.Core.Command;
using UnityEngine;

namespace Needle.Examples.Example_1
{
    public class Test : MonoBehaviour
    {

        void OnEnable() => CommandRegistry.RegisterInstance(this);
        void OnDisable() => CommandRegistry.UnregisterInstance(this);
        
        [ConsoleCommand("hello", "hello dear dev!")]
        [ParamDescriptor("some test description")]
        [ParamIdentifier("my first parameter")]
        public static void HelloWorld()
        {
            Debug.Log("Hello world!" );
        }
        
        
        [ConsoleCommand("echo", "hello dear dev!")]
        [ParamDescriptor("some test description")]
        [ParamIdentifier("my first parameter")]
        public void Echo(string param)
        {
            Debug.Log(param);
        }
    }
}