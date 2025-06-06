using System;

namespace NeedleAssets.Console.Core.Command
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ParamIdentifier : Attribute
    {
            public string Name { get; }

            public ParamIdentifier(string name) => Name = name;
    }
}