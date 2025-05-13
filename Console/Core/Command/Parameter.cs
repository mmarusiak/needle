using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Needle.Console.Core.Command
{
    public class Parameter
    {
        public ParameterInfo Info { get; }
        public string Name { get; }
        public string Description { get; }
        public bool Required { get; }
        public int ParameterCount { get; }
        public bool Generic => ParameterCount == 1;

        public Parameter(ParameterInfo info, string description): this (info, info.Name, description) { }

        public Parameter(ParameterInfo info, string name, string description)
        {
            Info = info;
            Name = name;
            Description = description;
            Required = !info.HasDefaultValue;
            ParameterCount = Mathf.Clamp(GetCount(), 1, int.MaxValue);
        }

        private int GetCount() => Info.ParameterType
                .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Select(ctor => ctor.GetParameters().Length)
                .DefaultIfEmpty(-1) // if no constructors exist
                .Min();
    }
}