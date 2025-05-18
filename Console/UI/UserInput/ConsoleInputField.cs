using TMPro;
using UnityEngine;

namespace NeedleAssets.Console.UI.UserInput
{ 
    [RequireComponent(typeof(Suggestions.Suggestions))]
    public class ConsoleInputField : TMP_InputField 
    {
        private Suggestions.Suggestions _suggestions;
        
        private new void Awake()
        {
            base.Awake();
            _suggestions = GetComponent<Suggestions.Suggestions>();    
        }
        
        public override void OnUpdateSelected(UnityEngine.EventSystems.BaseEventData eventData) 
        { 
            // handle mobile/gamepads inputs!!!!!!
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Tab)) 
            { 
                // handle suggestions/autocompletion
                return;
            }
            // add next suggestion
            base.OnUpdateSelected(eventData); // call default behavior otherwise
        }
    }
}
