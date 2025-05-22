using System;
using NeedleAssets.Console.Core.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NeedleAssets.Console.UI.CommandContext.Tooltip
{
    public class ConsoleTooltip<T> : MonoBehaviour where T : Enum
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI tooltipContent;
        [SerializeField] private Image panel;
        private RectTransform RectTransform => panel.rectTransform;
        
        private GameObject _content;
        
        protected virtual void Awake()
        {
            _content = transform.GetChild(0).gameObject;
        }
        
        private void SetHeader(string newHeader) => header.text = newHeader;
        private void SetTooltip(string newTooltip) => tooltipContent.text = newTooltip;
        protected virtual void Enable() => _content.SetActive(true);
        protected virtual void Disable() => _content.SetActive(false);
        
        public virtual void DisplayTooltip(int characterIndex)
        {
            var targetLog = NeedleConsole<T>.GetTargetLog(characterIndex);
            if (targetLog == null) return;
            
            // adjust pos of tooltip to mouse pos
            transform.position = Input.mousePosition - Vector3.up * RectTransform.sizeDelta.y / 1.25f;
            
            var content = NeedleConsole<T>.GetTooltipMessage(targetLog);
            if (content == null)
            {
                // we don't want to see tooltip if return value is null.
                Disable();
                return;
            }
            
            SetHeader("Log Context"); 
            SetTooltip(String.Join("\n", content));
            
            Enable();
        }
        
        public virtual void HideTooltip() => Disable();
    }
}