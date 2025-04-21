using System;

namespace Needle.Console.MethodsHandler
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ConsoleMethod : Attribute
    {
        public ConsoleMethod(string commandName, string name, string description, params string[] paramsDescription)
        {
            Container = new CommandContainer(commandName, name, description, paramsDescription);
        }

        public CommandContainer Container { get; }
    }
}