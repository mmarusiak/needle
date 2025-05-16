using System;
using System.Reflection;
using NeedleAssets.Console.Parser;
using NeedleAssets.Console.Utilities;

namespace NeedleAssets.Console.Core.Command
{
    public class Parameter
    {
        public ParameterInfo Info { get; }
        public string Name { get; }
        public string Description { get; }
        public bool Required { get; }
        public bool Generic { get; }
        
        // if field is not generic then it has Constructor, and constructor has some parameters!
        public Delegate Constructor { get; }
        public Parameter[] SubParameters { get; }
        

        public Parameter(ParameterInfo info, string description): this (info, info.Name, description) { }

        public Parameter(ParameterInfo info, string name, string description)
        {
            Info = info;
            Name = name;
            Description = description;
            Required = !info.HasDefaultValue;
            Generic = Utils.IsConvertibleFromString(info.ParameterType);
            if (Generic) return;
            // if no, let's get all 'sub parameters'
            Constructor = ParametersConstructors.CreateConstructorDelegate(info.ParameterType, out ParameterInfo[] constructorParams);
            SubParameters = new Parameter[constructorParams.Length];
            for (int i = 0; i < constructorParams.Length; i++)
                SubParameters[i] = new Parameter(constructorParams[i],
                    $"{constructorParams[i].ParameterType.Name} for {name}");
        }
    }
}