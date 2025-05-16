using System;
using System.Collections.Generic;
using NeedleAssets.Console.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NeedleAssets.Console.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LogText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        private TextMeshProUGUI _textMeshProUGUI;
        private bool _mouseIsOverText;
        private readonly List<Action<int>> _onHoverActions = new();
        private readonly List<Action> _onQuitHoverActions = new();

        private void Awake() => _textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseIsOverText = true;
            CheckHover(eventData.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseIsOverText = false;
            foreach (var action in _onQuitHoverActions) action();
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
                foreach (var action in _onQuitHoverActions) action();
                return;
            }

            TMP_CharacterInfo charInfo = _textMeshProUGUI.textInfo.characterInfo[visibleCharIndex];
            foreach (var listener in _onHoverActions) listener(charInfo.index);
        }

        public string Text
        {
            get => _textMeshProUGUI.text;
            set
            {
                _textMeshProUGUI.text = value;
                if (_textMeshProUGUI.rectTransform.sizeDelta.y > _textMeshProUGUI.preferredHeight) return;
                _textMeshProUGUI.rectTransform.sizeDelta = new Vector2(_textMeshProUGUI.rectTransform.sizeDelta.x, _textMeshProUGUI.preferredHeight);
            }
        }

        public void AddHoverListener(Action<int> listener) => _onHoverActions.Add(listener);
        public void RemoveHoverListener(Action<int> listener) => _onHoverActions.Remove(listener);
        
        public void AddQuitHoverListener(Action listener) => _onQuitHoverActions.Add(listener);
        public void RemoveQuitHoverListener(Action listener) => _onQuitHoverActions.Remove(listener);
    }
}