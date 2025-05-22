using System;
using NeedleAssets.Console.Core.Manager;
using NeedleAssets.Console.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NeedleAssets.Console.UI.CommandContext.Tooltip
{
    public class NeedleTooltip : ConsoleTooltip<MessageType>
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI tooltipContent;
        [SerializeField] private Image panel;
        private RectTransform RectTransform => panel.rectTransform;
        
        private GameObject _content;
        
        protected void Awake()
        {
            _content = transform.GetChild(0).gameObject;
        }
        
        protected override void SetHeader(string newHeader) => header.text = newHeader;
        protected override void SetTooltip(string newTooltip) => tooltipContent.text = newTooltip;
        protected override void Enable() => _content.SetActive(true);
        protected override void Disable() => _content.SetActive(false);
        
        public override void DisplayTooltip(int characterIndex)
        {
            var targetLog = NeedleConsole<MessageType>.GetTargetLog(characterIndex);
            if (targetLog == null) return;
            
            // adjust pos of tooltip to mouse pos
            transform.position = Input.mousePosition - Vector3.up * RectTransform.sizeDelta.y / 1.25f;
            
            var content = NeedleConsole<MessageType>.GetTooltipMessage(targetLog);
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
        
        public override void HideTooltip() => Disable();
    }
}