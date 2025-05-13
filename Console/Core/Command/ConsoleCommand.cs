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

        // let's say that our parameter is some custom class
        // f.e. Vector3
        // it means that there is no simple way to convert "generic" type to custom class
        // what we want to do:
        // check if there is "CommandConstructor" - if no, take *some* default constructor - with min params?
        // then get all params from constructor and parse them as new parameters for function
        // then when parsing we want to detect that those parameters are for creating special not generic type
        // void Test(Vector3 param) => void TestPacker(float x, float y, float z) [maybe get constructor with max parameters? or register all candidates and check the correct one?]
        // => TestPacker calls Test(new(x, y, z))
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