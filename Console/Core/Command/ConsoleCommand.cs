using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace Needle.Console.Core.Command
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ConsoleCommand : Attribute
    {
        // static method by default
        private object _source;
        public string Name { get; }
        public string Description { get; }
        private Parameter[] _parameters = null;
        private MethodInfo _method;

        public ConsoleCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void RegisterMethod(MethodInfo method, ParamIdentifier identifier, ParamDescriptor descriptor)
        {
            _method = method;
            RegisterParameters(method.GetParameters(), identifier, descriptor);
        }

        private void RegisterParameters(ParameterInfo[] parameters, ParamIdentifier identifier, ParamDescriptor descriptor)
        {
            _parameters = new Parameter[parameters.Length];
            ParameterInfo paramInfo = parameters.Length != 0 ? parameters[0] : null;
            for (int i = 0; i < parameters.Length; paramInfo = parameters[i++])
            {
                Assert.IsNotNull(paramInfo, "Param info should not be null!");
                string description = (descriptor != null && descriptor.Length > i) ? descriptor.Get(i) : string.Empty;
                string name = identifier == null ? paramInfo.Name : identifier.Name;
                _parameters[i] = new Parameter(paramInfo, name, description);
            }
        }
        
        public void RegisterSource(object source) => _source = source;
        
        public object Source => _source;
        public Parameter[] Parameters => _parameters;
        public MethodInfo Method => _method;
    }
}