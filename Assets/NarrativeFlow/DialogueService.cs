using UnityEngine;

namespace NarrativeFlow
{
    public class DialogueService : MonoBehaviour
    {
        public DialogueRoot DialogueRoot;
       
        private DialogContent _currentDialogue;

        void Start()
        {
            if (DialogueRoot != null)
            {
                DialogueRoot.Initialize();
                StartDialogue("NewTextFile.narr");
            }
        }

        public void StartDialogue(string dialogueName)
        {
            _currentDialogue = DialogueRoot.GetDialogue(dialogueName);
            if (_currentDialogue != null)
            {
                DisplayDialogue();
                ExecuteCommands();
            }
        }

        private void DisplayDialogue()
        {
            Debug.Log(_currentDialogue.Dialogue);
        }

        private void ExecuteCommands()
        {
            foreach (var command in _currentDialogue.Commands)
            {
                var commandParts = command.Split(' ');
                var commandType = commandParts[0];
                var commandValue = commandParts.Length > 1 ? commandParts[1] : null;

                switch (commandType)
                {
                    case "change_background":
                        Debug.Log("Changing background to " + commandValue);
                        break;
                    case "play_music":
                        Debug.Log("Playing music: " + commandValue);
                        break;
                    case "next_event":
                        StartDialogue(commandValue);
                        break;
                    default:
                        Debug.LogWarning("Unknown command: " + command);
                        break;
                }
            }
        }
    }
}