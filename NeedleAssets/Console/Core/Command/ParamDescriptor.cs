using System;

namespace NeedleAssets.Console.Core.Command
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ParamDescriptor : Attribute
    {
        private readonly string[] _descriptions;

        public ParamDescriptor(params string[] descriptions) => _descriptions = descriptions;
        
        public int Length => _descriptions.Length;
        public string Get(int i) => _descriptions[i];
    }
}