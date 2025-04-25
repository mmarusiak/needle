using System.Reflection;

namespace Needle.Console.MethodsHandler
{
    public class Command
    {
        public CommandContainer Container { get; }
        public MethodInfo Method { get; }
        public object TargetInstance { get; }

        public Command(CommandContainer container, MethodInfo method, object targetInstance = null)
        {
            Container = container;
            Method = method;
            TargetInstance = targetInstance;
        }

        public string GetInfo()
        {
            string r = $"{Container.Name}:\n\tTo call method type \'{Container.Command}\'\n\t{Container.Description}";
            if (Method.GetParameters().Length > 0) r += "\n\tParams:";
            for (int i = 0; i < Method.GetParameters().Length && i < Container.ParamsDescription.Length; r += $"\n\t\t{Method.GetParameters()[i].ParameterType.Name} {Method.GetParameters()[i].Name}: {Container.ParamsDescription[i++]}");
            return r;
        }
    }
}