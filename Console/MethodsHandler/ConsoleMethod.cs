using System;

namespace Needle.Console.MethodsHandler
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ConsoleMethod : Attribute
    {
        private string _name, _description;
        
        public ConsoleMethod(string name, string description)
        {
            _name = name;
            _description = description;
        }

        public string GetName() => _name;
        public string GetDescription() => _description;
    }
}