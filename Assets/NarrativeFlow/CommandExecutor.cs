using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NarrativeFlow
{
    public class CommandExecutor : MonoBehaviour
    {
        [SerializeField] private TMP_Text _speakerText;
        [SerializeField] private DialogueService _dialogueService;
        [SerializeField] private float _transitionDelay = 1.0f;

        public float TransitionDelay => _transitionDelay;

        public void ExecuteCommand(string command)
        {
            var commandParts = command.Split(' ');
            var commandType = commandParts[0].Substring(1);
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
                    StartCoroutine(ExecuteNextEventWithDelay(commandValue));
                    break;
                default:
                    Debug.LogWarning("Unknown command: " + command);
                    break;
            }
        }

        private IEnumerator ExecuteNextEventWithDelay(string nextEvent)
        {
            yield return new WaitForSeconds(_transitionDelay);
            _dialogueService.StartDialogue(nextEvent);
        }
    }
}