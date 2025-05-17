using NeedleAssets.Console.Utilities;
using UnityEngine;

namespace NeedleAssets.Console.Core
{
    public class NeedleBehaviour : BonoBehaviour
    {
        protected override void OnStartAndEnable()
        {
            Debug.Log("OnStartAndEnable");
            CommandRegistry.RegisterInstance(this);
        }

        public void OnDisable() => CommandRegistry.UnregisterInstance(this);
    }
}