using System.Collections.Generic;
using NeedleAssets.Console.Core.Command;
using NeedleAssets.Console.Core.Manager;
using NeedleAssets.Console.UI.Entries;
using NeedleAssets.Console.Utilities;
using UnityEngine;

namespace NeedleAssets.Console
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
    }
}