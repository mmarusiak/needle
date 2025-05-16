using System;
using System.Collections.Generic;
using NeedleAssets.Console.Core.Command;
using UnityEngine;

namespace NeedleAssets.Console.Parser
{
    public class ParameterParser
    {
        public object[] Result { get; }
        public string Error { get; }
        public bool Success { get; }

        private int _index = 0;
        private int _paramIndex = 0;
        private int _parametersLength;

        public ParameterParser(string[] args, Parameter[] parameters)
        {
            foreach (var param in parameters)
            {
                CalculateParametersCount(param);
            }
            
            Success = ParseParameters(args, parameters, out object[] result, out string error);
            Result = result;
            Error = error;
        }

        private void CalculateParametersCount(Parameter param)
        { 
            if (param.Generic) _parametersLength++;
            else
            {
                foreach (var p in param.SubParameters) CalculateParametersCount(p);
            }
        }
        
        private bool ParseParameters(string[] args, Parameter[] parameters, out object[] result, out string error)
        {
            // how to parse vectors/colors other types?
            // here parse!!!
            List<object> outArgs = new List<object>();
            // maybe loop through all parameters and sub parameters? through 'mega count'
            // store current param index
            for (; _index < _parametersLength; _index++)
            {
                 // check if is generic
                 // if is then do what we do
                 // if not then we should:
                 // then take i - param index parameter and that's our parameter
                 // what if this parameter is not generic?
                 // maybe... take args from i to end and call ParseParameters with this parameter - try to get object
                 // add some index callout
                 // add to outArgs
                var param = parameters[_paramIndex];
                if (args.Length <= _index && !param.Required) outArgs.Add(param.Info.DefaultValue);
                else if (args.Length <= _index)
                {
                    error = $"Expected at least  {_index + 1} (count of parameters with possible optional ones: {_parametersLength}) required parameters but got {args.Length}! Error while trying to parse arg {param.Name}!";
                    result = null;
                    return false;
                }
                else if (param.Generic)
                {
                    try
                    {
                        outArgs.Add(Convert.ChangeType(args[_index], param.Info.ParameterType));
                        _paramIndex++;
                    }
                    catch (Exception ex)
                    {
                        error =
                            $"Error while trying to parse arg {param.Name}:{param.Info.ParameterType}, provided value: \'{args[_index]}\'. {ex.Message}";
                        result = null;
                        return false;
                    }
                }
                else
                {
                    ParameterParser parser = new ParameterParser(args[_index..], param.SubParameters);
                    if (parser.Success)
                    {
                        _index += parser._index - 1;
                        _paramIndex++;
                        var f = (Func<object[], object>) param.Constructor;
                        var obj = f(parser.Result);
                        outArgs.Add(obj);
                    }
                    else
                    {
                        error = parser.Error;
                        result = null;
                        return false;
                    }
                }
            }
            result = outArgs.ToArray();
            error = "";
            return true;
        }
    }
}