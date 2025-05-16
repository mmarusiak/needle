using System.Collections.Generic;
using Needle.Console.Core.Command;
using Needle.Console.Logger;
using Needle.Console.UI.Entries;
using UnityEngine;

namespace Needle.Console
{
    public class Needle : NeedleConsole<MessageType>
    {
        protected override Dictionary<MessageType, Color> TypeToColors => new ()
        {
            {MessageType.Info, NeedleColors.Colors[0]},
            {MessageType.Warning, NeedleColors.Colors[1]},
            {MessageType.Error, NeedleColors.Colors[2]},
            {MessageType.Debug, NeedleColors.Colors[3]},
            {MessageType.UserInput, NeedleColors.Colors[4]},
        };

        protected override MessageType Info => MessageType.Info;
        protected override MessageType Warning => MessageType.Warning;
        protected override MessageType Error => MessageType.Error;
        protected override MessageType Debug => MessageType.Debug;
        protected override MessageType Input => MessageType.UserInput;

        protected override IEntryLogger<MessageType> MessageLogger() => new NeedleEntryLogger<MessageType>();

        [ConsoleCommand("echo", "Echo")]
        public static string Echo(string message) => message;
    }
}