using System.Reflection;

namespace Needle.Console.Core
{
    public class Parameter
    {
        private ParameterInfo _info;
        private string _name;
        private string _description;
        private bool _required;

        public Parameter(ParameterInfo info, string description): this (info, info.Name, description) { }

        public Parameter(ParameterInfo info, string name, string description)
        {
            _info = info;
            _name = name;
            _description = description;
            _required = !info.HasDefaultValue;
        }
    }
}