using Needle.Console.Core;
using Needle.Console.Core.Command;

namespace Needle.Examples.Example_1
{
    public class Test
    {
        [ConsoleCommand("hello world", "some description")]
        [ParamDescriptor("some test description")]
        public static void HelloWorld(string param)
        {
            
        }
    }
}