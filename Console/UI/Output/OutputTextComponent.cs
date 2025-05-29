using System;
using NeedleAssets.Console.UI.Entries;
using NeedleAssets.Console.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NeedleAssets.Console.UI.Output
{
    public class OutputTextComponent<T> : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler where T : Enum
    {
        [SerializeField] private UnityEvent<ConsoleLogEntry<T>> onEnterHover;
        [SerializeField] private UnityEvent onQuitHover;
        [SerializeField] private UnityEvent<ConsoleLogEntry<T>> onClick;
        [SerializeField] private UnityEvent onRectResized;
        
        private TextMeshProUGUI _textMeshProUGUI;
        private bool _mouseIsOverText;
        private ConsoleUI<T> _consoleUI;
        
        protected virtual void Awake() => _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        
        public void RegisterUI(ConsoleUI<T> consoleUI) => _consoleUI = consoleUI; 

        public void OnPointerEnter(PointerEventData eventData) => _mouseIsOverText = true;

        public void OnPointerExit(PointerEventData eventData) => _mouseIsOverText = false;

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_mouseIsOverText) onEnterHover?.Invoke(CheckHover(eventData.position));
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_mouseIsOverText) onClick?.Invoke(CheckHover(eventData.position));
        }
        
        private ConsoleLogEntry<T> CheckHover(Vector2 mousePosition)
        {
            _textMeshProUGUI.ForceMeshUpdate();
            int visibleCharIndex = Utils.GetNearestCharacterWithMaxDistance(_textMeshProUGUI, mousePosition, 10);
            if (visibleCharIndex == -1)
            {
                onQuitHover?.Invoke();
                return null;
            }

            TMP_CharacterInfo charInfo = _textMeshProUGUI.textInfo.characterInfo[visibleCharIndex];
            return _consoleUI.GetTargetLog(charInfo.index);
        }
    }
}