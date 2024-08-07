using UnityEngine;

namespace NarrativeFlow
{
    public class DialogueService : MonoBehaviour
    {
        [SerializeField] private DialogueRoot _dialogueRoot;
        [SerializeField] private TextDisplayer _textDisplayer;

        private DialogContent _currentDialogue;
        public DialogContent CurrentDialogue => _currentDialogue;

        void Start()
        {
            if (_dialogueRoot != null)
            {
                _dialogueRoot.Initialize();
                StartDialogue("NewTextFile");
            }
        }

        public void StartDialogue(string dialogueName)
        {
            _currentDialogue = _dialogueRoot.GetDialogue(dialogueName);
            if (_currentDialogue != null)
            {
                _textDisplayer.DisplayText(_currentDialogue);
            }
        }
    }
}