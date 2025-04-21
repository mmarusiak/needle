using System.Reflection;

namespace Needle.Console.MethodsHandler
{
    public class MethodContainer
    {
        private string _name, _description;
        private MethodInfo _method;

        public MethodContainer(string name, string description, MethodInfo method)
        {
            _name = name;
            _description = description;
            _method = method;
        }

        public string GetName() => _name;
        public string GetDescription() => _description;
        public MethodInfo GetMethod() => _method;
    }
}