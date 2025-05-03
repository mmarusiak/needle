using System.Reflection;

namespace Needle.Console.Core.Command
{
    public class Parameter
    {
        public ParameterInfo Info { get; }
        public string Name { get; }
        public string Description { get; }
        public bool Required { get; }

        public Parameter(ParameterInfo info, string description): this (info, info.Name, description) { }

        public Parameter(ParameterInfo info, string name, string description)
        {
            Info = info;
            Name = name;
            Description = description;
            Required = !info.HasDefaultValue;
        }
    }
}