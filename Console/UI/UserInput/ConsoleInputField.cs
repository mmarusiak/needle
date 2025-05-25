using TMPro;
using UnityEngine;

namespace NeedleAssets.Console.UI.UserInput
{ 
    [RequireComponent(typeof(Suggestions.Suggestions))]
    public class ConsoleInputField : TMP_InputField 
    {
        private Suggestions.Suggestions _suggestions;
        [SerializeField] private string originalPlaceholderLabel;
        private TextMeshProUGUI _placeholder;
        
        private new void Awake()
        {
            base.Awake();
            _suggestions = GetComponent<Suggestions.Suggestions>();
            _placeholder = placeholder.GetComponent<TextMeshProUGUI>();
            if (_placeholder.text != string.Empty && originalPlaceholderLabel == string.Empty) originalPlaceholderLabel = _placeholder.text;
            else _placeholder.text = originalPlaceholderLabel;
        }
        
        public override void OnUpdateSelected(UnityEngine.EventSystems.BaseEventData eventData) 
        { 
            // handle mobile/gamepads inputs!!!!!!
            if (Input.GetKeyDown(KeyCode.UpArrow)) 
            { 
                _suggestions.UpSelection();
                _placeholder.text = _suggestions.GetCurrentSuggestionSilently().Name;
                placeholder.enabled = true;
                return;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _suggestions.DownSelection();
                _placeholder.text = _suggestions.GetCurrentSuggestionSilently().Name;
                placeholder.enabled = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                var output = _suggestions.GetCurrentSuggestion();
                text = output.Name;
                caretPosition = text.Length;
                _placeholder.text = originalPlaceholderLabel;
                return;
            }
            // to prevent unfocusing
            if (Input.GetKeyDown(KeyCode.Return))
            {
                onEndEdit.Invoke(text);
                return;
            }
            base.OnUpdateSelected(eventData); // call default behavior otherwise
        }
        
        public void OnEdited() => _placeholder.text = originalPlaceholderLabel;
        
        public void Clear() => text = "";
    }
}
