using System;
using System.Reflection;
using UnityEngine;
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

        // TO DO:
        // [1 X - done]. Add object name to function caller - by default it will be called to all object
        // why? if dev wants to call command only for some objects with specified name
        // STATIC/RUNTIME keywords
        // move [player] 10, 10, 20 => means calling move only on player game object by some params
        // move [player, slayer] 10, 10, 20 => means calling move on player and slayer
        // move [STATIC] 10, 10, 20 => means calling move on statics only
        // move [RUNTIME] 10, 10, 20 => means calling move on all Mono Behaviours runtime scripts - game objects
        // move 10, 10, 20 => means calling move globally, on every registered instance
        // 2. Handle not generic parameter types
        // let's say that our parameter is some custom class
        // f.e. Vector3
        // it means that there is no simple way to convert "generic" type to custom class
        // what we want to do:
        // check if there is "CommandConstructor" - if no, take *some* default constructor - with min params?
        // then get all params from constructor and parse them as new parameters for function
        // then when parsing we want to detect that those parameters are for creating special not generic type
        // void Test(Vector3 param) => void TestPacker(float x, float y, float z) [maybe get constructor with max parameters? or register all candidates and check the correct one?]
        // => TestPacker calls Test(new(x, y, z))
        // it will be hard to detect how many parameters are needed for type - f.e. you can declare int like that: "int a = new int();"
        // which corresponds to: "int a = 0;" how to be sure if int has no parameters, or if it should take some?
        // I mean there are not many "generic types", but I'm not sure if we want to hardcode these types somewhere....
        // Another relevant issue:
        // what if some "not generic class" in constructor takes another "not generic class" as parameter and there is no other constructor to handle this?
        // f.e. SuperVector vec = new SuperVector(Vector2 v2, Vector3 v3, Vector4 v4);
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
        
        public void RegisterSource(object source) => _source = source;
        
        public object Source => _source;
        public Parameter[] Parameters => _parameters;
        public MethodInfo Method => _method;
    }
}