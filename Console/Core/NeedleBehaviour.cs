using NeedleAssets.Console.Core.Registry;
using NeedleAssets.Console.Utilities;

namespace NeedleAssets.Console.Core
{
    public class NeedleBehaviour : BonoBehaviour
    {
        protected override void OnStartAndEnable() => CommandRegistry.RegisterInstance(this);
        

        public void OnDisable() => CommandRegistry.UnregisterInstance(this);
    }
}