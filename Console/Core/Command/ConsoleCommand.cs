using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace NeedleAssets.Console.Core.Command
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ConsoleCommand : Attribute
    {
        // static method by default
        private object _source;
        public string Name { get; }
        public string Description { get; }
        public bool DevCommand { get; }
        private Parameter[] _parameters;
        private MethodInfo _method;

        public ConsoleCommand(string name, string description, bool devCommand = false)
        {
            Name = name;
            Description = description;
            DevCommand = devCommand;
        }

        public void RegisterMethod(MethodInfo method, ParamIdentifier identifier, ParamDescriptor descriptor)
        {
            _method = method;
            RegisterParameters(method.GetParameters(), identifier, descriptor);
        }
        
        private void RegisterParameters(ParameterInfo[] parameters, ParamIdentifier identifier, ParamDescriptor descriptor)
        {
            _parameters = new Parameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var paramInfo = parameters[i];
                Assert.IsNotNull(paramInfo, "Param info should not be null!");
                string description = (descriptor != null && descriptor.Length > i) ? descriptor.Get(i) : string.Empty;
                string name = identifier == null ? paramInfo.Name : identifier.Name;
                _parameters[i] = new Parameter(paramInfo, name, description);
            }
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            var other = (ConsoleCommand)obj;
            return (other.Source == Source && other.Name == Name);
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => $"{Name} : {Description}, from {Source} dev mode: {DevCommand}";

        public void RegisterSource(object source) => _source = source;
        
        public object Source => _source;
        public Parameter[] Parameters => _parameters;
        public MethodInfo Method => _method;
    }
}