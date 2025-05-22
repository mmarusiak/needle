using System;
using UnityEngine;

namespace NeedleAssets.Console.UI.CommandContext.Tooltip
{
    public abstract class ConsoleTooltip<T> : MonoBehaviour where T : Enum
    {
        protected abstract void SetHeader(string newHeader);
        protected abstract void SetTooltip(string newTooltip);
        protected abstract void Enable();
        protected abstract void Disable();

        public abstract void DisplayTooltip(int characterIndex);
        
        public virtual void HideTooltip() => Disable();
    }
}