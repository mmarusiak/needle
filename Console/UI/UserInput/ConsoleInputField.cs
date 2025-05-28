using TMPro;
using UnityEngine;

namespace NeedleAssets.Console.UI.UserInput
{ 
    [RequireComponent(typeof(Suggestions.Suggestions))]
    public class ConsoleInputField : TMP_InputField 
    {
        private const int StackSize = 6;
        private int _currentStackSize = 0;
        private string[] _entriesStack = new string[StackSize];
        private int _stackLookUp = -1;
        
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
            // to prevent unfocusing
            if (Input.GetKeyDown(KeyCode.UpArrow)) 
            { 
                if (text == "")
                {
                    if (_currentStackSize == 0) return;
                    _stackLookUp += 1;
                    _stackLookUp %= _currentStackSize;
                    _placeholder.text = _entriesStack[_currentStackSize - 1 - _stackLookUp];
                    placeholder.enabled = true;
                    return;
                }
                _suggestions.UpSelection();
                _placeholder.text = _suggestions.GetCurrentSuggestionSilently().Name;
                placeholder.enabled = true;
                return;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (text == "")
                {
                    if (_currentStackSize == 0) return;
                    if (_stackLookUp != -1) _stackLookUp -= 1;
                    _placeholder.text = _stackLookUp >= 0 ? _entriesStack[_currentStackSize - 1 - _stackLookUp] : originalPlaceholderLabel;
                    placeholder.enabled = _stackLookUp >= 0;
                    // get last from stack
                    // draw placeholder and if tabbed then get it
                    // use peek
                    // maybe get only five/six last commands
                    return;
                }
                _suggestions.DownSelection();
                if (_suggestions.GetCurrentSuggestionSilently() == null) return;
                _placeholder.text = _suggestions.GetCurrentSuggestionSilently().Name;
                placeholder.enabled = true;
                return;
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (text == "" && _stackLookUp != -1 && _currentStackSize != 0)
                {
                    text = _entriesStack[_currentStackSize - 1 - _stackLookUp];
                    caretPosition = text.Length;
                    return;
                }
                var output = _suggestions.GetCurrentSuggestion();
                text = output.Name;
                caretPosition = text.Length;
                _placeholder.text = originalPlaceholderLabel;
                return;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                onEndEdit.Invoke(text);
                return;
            }
            base.OnUpdateSelected(eventData); // call default behavior otherwise
        }
        
        public void OnEdited() => _placeholder.text = originalPlaceholderLabel;
        
        public void Clear() => text = "";

        public void AddEntryToStack(string entry)
        {
            if (_entriesStack.Length == StackSize) _entriesStack = _entriesStack[1..];
            if (_currentStackSize < StackSize) _currentStackSize++;
            _entriesStack[_currentStackSize - 1] = entry;
        }
    }
}
