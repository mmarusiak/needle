using UnityEngine;

namespace NeedleAssets.Console.Core
{
    public class NeedleBehaviour : MonoBehaviour
    {
        public void OnEnable() => CommandRegistry.RegisterInstance(this);
        public void OnDisable() => CommandRegistry.UnregisterInstance(this);
    }
}