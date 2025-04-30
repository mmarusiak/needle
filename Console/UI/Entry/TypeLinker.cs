using System;
using System.Collections.Generic;
using UnityEngine;

namespace Needle.Console.UI.Entry
{
    public class TypeLinker<T> where T : Enum
    {
        public Dictionary<T, Color32> TypeColor { get; }
        
        public TypeLinker(Dictionary<T, Color32> typeColor) => TypeColor = typeColor;
    }
}