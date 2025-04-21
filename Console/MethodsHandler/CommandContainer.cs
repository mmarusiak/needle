namespace Needle.Console.MethodsHandler
{
    public class CommandContainer
    {
        public string Command { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] ParamsDescription { get; set; }

        public CommandContainer(string command, string name, string description, params string[] paramsDescription)
        {
            Command = command;
            Name = name;
            Description = description;
            ParamsDescription = paramsDescription;
        }
    }
}