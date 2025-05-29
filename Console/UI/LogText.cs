using NeedleAssets.Console.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NeedleAssets.Console.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LogText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        private TextMeshProUGUI _textMeshProUGUI;
        private bool _mouseIsOverText;
        
        
        [SerializeField] private UnityEvent<int> onHover;
        [SerializeField] private UnityEvent onQuitHover;
        [SerializeField] private UnityEvent onTextRectResized;

        private void Awake() => _textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseIsOverText = true;
            CheckHover(eventData.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseIsOverText = false;
            onQuitHover?.Invoke();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_mouseIsOverText)
            {
                CheckHover(eventData.position);
            }
        }

        private void CheckHover(Vector2 mousePosition)
        {
            _textMeshProUGUI.ForceMeshUpdate();
            int visibleCharIndex = Utils.GetNearestCharacterWithMaxDistance(_textMeshProUGUI, mousePosition, 10);
            if (visibleCharIndex == -1)
            {
                onQuitHover?.Invoke();
                return;
            }

            TMP_CharacterInfo charInfo = _textMeshProUGUI.textInfo.characterInfo[visibleCharIndex];
            onHover?.Invoke(charInfo.index);
        }

        public string Text
        {
            get => _textMeshProUGUI.text;
            set
            {
                _textMeshProUGUI.text = value;
                if (_textMeshProUGUI.rectTransform.sizeDelta.y > _textMeshProUGUI.preferredHeight) return;
                _textMeshProUGUI.rectTransform.sizeDelta = new Vector2(_textMeshProUGUI.rectTransform.sizeDelta.x, _textMeshProUGUI.preferredHeight);
                onTextRectResized?.Invoke();
            }
        }

        public void AddHoverListener(UnityAction<int> listener) => onHover.AddListener(listener);
        public void RemoveHoverListener(UnityAction<int> listener) => onHover.RemoveListener(listener);
        
        public void AddQuitHoverListener(UnityAction listener) => onQuitHover.AddListener(listener);
        public void RemoveQuitHoverListener(UnityAction listener) => onQuitHover.RemoveListener(listener);
    }
}