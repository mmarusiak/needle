using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Needle.Console.UI
{
    public class ConsoleTooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI header;
        [SerializeField] private TextMeshProUGUI tooltipContent;
        [SerializeField] private Image panel;
        
        public void SetHeader(string newHeader) => header.text = newHeader;
        public void SetTooltip(string newTooltip) => tooltipContent.text = newTooltip;
        public void Enable() => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);
        
        public RectTransform RectTransform => panel.rectTransform;
    }
}