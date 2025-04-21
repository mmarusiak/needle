using System;

namespace Needle.Console.MethodsHandler
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ConsoleMethod : Attribute
    {
        private CommandContainer _container;
        
        public ConsoleMethod(string commandName, string name, string description, params string[] paramsDescription)
        {
            _container = new CommandContainer(commandName, name, description, paramsDescription);
        }

        public CommandContainer Container => _container;
    }
}