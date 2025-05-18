using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NeedleAssets.Console.Core.Parser
{
    public static class ParametersConstructors
    {
        public static Delegate CreateConstructorDelegate(Type type, out ParameterInfo[] parameterTypes)
        {
            var constructors = type.GetConstructors();
            var ctor = GetConstructor(constructors);

            if (ctor == null)
                throw new InvalidOperationException($"No public constructor found for type {type}");

            parameterTypes = ctor.GetParameters();

            var paramExpr = Expression.Parameter(typeof(object[]), "args");
            Expression[] argumentExpressions = ctor.GetParameters()
                .Select((p, i) =>
                    Expression.Convert(
                        Expression.ArrayIndex(paramExpr, Expression.Constant(i)),
                        p.ParameterType
                    )).ToArray<Expression>();

            var newExpr = Expression.New(ctor, argumentExpressions);
            var lambda = Expression.Lambda(typeof(Func<object[], object>), Expression.Convert(newExpr, typeof(object)), paramExpr);

            return lambda.Compile();
        }
        
        private static ConstructorInfo GetConstructor(ConstructorInfo[] constructors)
        {
            ConstructorInfo current = null;
            int len = 0;

            foreach (var c in constructors)
            {
                if (c.GetCustomAttribute<CommandConstructor>() != null) return c;
                if (current == null || len < c.GetParameters().Length)
                {
                    len = c.GetParameters().Length;
                    current = c;
                }
            }
            
            return current;
        }
    }
}