using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace Needle.Console.Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class Command : Attribute
    {
        // static method by default
        private object _source = null;
        private string _name;
        private string _description;
        private Parameter[] _parameters;
        private MethodInfo _method;

        public Command(string name, string description, params Parameter[] parameters)
        {
            _name = name;
            _description = description;
            _parameters = parameters;
        }

        public void RegisterMethod(MethodInfo method, ParamDescriptor descriptor)
        {
            _method = method;
            RegisterParameters(method.GetParameters(), descriptor);
        }

        private void RegisterParameters(ParameterInfo[] parameters, ParamDescriptor descriptor)
        {
            _parameters = new Parameter[parameters.Length];
            ParameterInfo paramInfo = null;
            for (int i = 0; i < parameters.Length; paramInfo = parameters[i++])
            {
                Assert.IsNull(paramInfo, "Param info should not be null!");
                string description = (descriptor != null && descriptor.Length > i) ? descriptor.Get(i) : string.Empty;
                _parameters[i] = new Parameter(paramInfo, description);
            }
        }
    }
}