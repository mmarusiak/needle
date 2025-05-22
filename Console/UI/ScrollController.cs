using UnityEngine;
using UnityEngine.UI;

namespace NeedleAssets.Console.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollController : MonoBehaviour
    {
        private ScrollRect _scrollRect;
        
        private void Awake() => _scrollRect = GetComponent<ScrollRect>();
        
        public void ScrollDown() => _scrollRect.verticalNormalizedPosition = 0;
    }
}