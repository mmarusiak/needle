using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Needle.Console.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LogText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
    {
        public TextMeshProUGUI textMeshProUGUI;
        public Camera uiCamera;

        private bool _mouseIsOverText = false;

        private readonly List<Action<int>> _hoverActions = new();

        void Awake() => textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        public void OnPointerEnter(PointerEventData eventData)
        {
            _mouseIsOverText = true;
            CheckHover(eventData.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _mouseIsOverText = false;
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
            textMeshProUGUI.ForceMeshUpdate();
            int visibleCharIndex =
                TMP_TextUtilities.FindIntersectingCharacter(textMeshProUGUI, mousePosition, null, false);
            if (visibleCharIndex == -1) return;
            TMP_CharacterInfo charInfo = textMeshProUGUI.textInfo.characterInfo[visibleCharIndex];
            foreach (var listener in _hoverActions) listener(charInfo.index);
        }

        public string Text
        {
            get => textMeshProUGUI.text;
            set
            {
                Debug.Log(value);
                textMeshProUGUI.text = value;
            }
        }

        public void AddListener(Action<int> listener) => _hoverActions.Add(listener);
        public void RemoveListener(Action<int> listener) => _hoverActions.Remove(listener);
    }
}