namespace Needle.Console.Logger
{
    public class Message
    {
        public string Content { get; }
        public MessageType Type { get; }

        public Message(string content, MessageType type)
        {
            Content = content;
            Type = type;
        }
    }
}