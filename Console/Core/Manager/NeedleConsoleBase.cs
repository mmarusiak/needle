using UnityEngine;

namespace NeedleAssets.Console.Core.Manager
{
    public abstract class NeedleConsoleBase : MonoBehaviour
    {
        private static NeedleConsoleBase _instance;

        public virtual void Awake() => _instance = this;
        
        protected virtual bool DeveloperMode()
        {
#if UNITY_EDITOR
            return true;
#endif
            return false;
        }
        
        public static bool InDeveloperMode => _instance.DeveloperMode();
    }
}