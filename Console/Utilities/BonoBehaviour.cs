using System;
using UnityEngine;

namespace NeedleAssets.Console.Utilities
{
    public abstract class BonoBehaviour : MonoBehaviour
    {
        [NonSerialized]
        private bool _initialized;

        private void Start()
        {
            _initialized = true;
            OnStart();
            OnStartAndEnable();
        }

        private void OnEnable()
        {
            if (!_initialized) return;
            OnStartAndEnable();
        }

        protected virtual void OnStart()
        {
            
        }
        
        protected virtual void OnStartAndEnable()
        {
            
        }
    }
}