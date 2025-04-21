namespace Needle.Console.MethodsHandler
{
    public abstract class Method
    {
        private string _name, _description;

        public Method(string name, string description)
        {
            _name = name;
            _description = description;
        }
        
        public string GetName() => _name;
        public string GetDescription() => _description;
    }
}