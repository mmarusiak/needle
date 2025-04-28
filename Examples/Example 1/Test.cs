using Needle.Console.Core;

namespace Needle.Examples.Example_1
{
    public class Test
    {
        [Command("hello world", "some description")]
        [ParamDescriptor("some test description")]
        public static void HelloWorld(string param)
        {
            
        }
    }
}