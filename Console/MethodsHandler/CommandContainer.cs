namespace Needle.Console.MethodsHandler
{
    public class CommandContainer
    {
        public string Command { get; }
        public string Name { get; }
        public string Description { get; }
        public string[] ParamsDescription { get; }

        public CommandContainer(string command, string name, string description, params string[] paramsDescription)
        {
            Command = command;
            Name = name;
            Description = description;
            ParamsDescription = paramsDescription;
        }
    }
}