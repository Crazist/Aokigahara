using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NarrativeFlow
{
    public class TextDisplayer : MonoBehaviour
    {
        [SerializeField] private DialogueService _dialogueService;
        [SerializeField] private CommandExecutor _commandExecutor;
        [SerializeField] private Button _continueButton;
        [SerializeField] private TMP_Text _dialogueText;
        [SerializeField] private TMP_Text _speakerText;
        [SerializeField] private int _maxCharactersPerPage = 100;

        private Queue<string> _pages;
        private TextSplitter _textSplitter;
        private bool _isWaitingForInput;
        private bool _shouldClearSpeaker;

        private void Awake()
        {
            _textSplitter = new TextSplitter();
            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _continueButton.gameObject.SetActive(false);
        }

        public void DisplayText(DialogContent dialogContent)
        {
            StopAllCoroutines();
            _dialogueText.text = "";
            _speakerText.text = "";
            _shouldClearSpeaker = false;
            _pages = new Queue<string>(_textSplitter.SplitTextIntoPages(dialogContent.Dialogue, _maxCharactersPerPage));
            DisplayNextPage(dialogContent.DisplaySpeed);
        }

        private void OnContinueButtonClicked()
        {
            if (_isWaitingForInput)
            {
                _isWaitingForInput = false;
                _continueButton.gameObject.SetActive(false);
                if (_shouldClearSpeaker)
                {
                    _speakerText.text = "";
                    _shouldClearSpeaker = false;
                }
                DisplayNextPage(0.05f);
            }
        }

        private void DisplayNextPage(float speed)
        {
            if (_pages.Count > 0)
            {
                string page = _pages.Dequeue();
                if (page.StartsWith("#"))
                {
                    ExecuteCommand(page);
                }
                else
                {
                    StartCoroutine(TypeText(page, speed));
                }
            }
            else
            {
                _continueButton.gameObject.SetActive(false);
                StartCoroutine(ExecuteCommandsWithDelay());
            }
        }

        private IEnumerator TypeText(string text, float speed)
        {
            _dialogueText.text = "";
            _isWaitingForInput = false;

            bool isInDialogue = false;

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '"' && !isInDialogue)
                {
                    isInDialogue = true;
                    _dialogueText.text += "\n";
                }
                else if (text[i] == '"' && isInDialogue)
                {
                    isInDialogue = false;
                }
                else if (!isInDialogue && text[i] == ':')
                {
                    string speakerName = _dialogueText.text.Trim();
                    _speakerText.text = speakerName;
                    _dialogueText.text = "";
                }
                else
                {
                    _dialogueText.text += text[i];
                    yield return new WaitForSeconds(speed);
                }
            }

            if (_pages.Count > 0)
            {
                _isWaitingForInput = true;
                _continueButton.gameObject.SetActive(true);
                _shouldClearSpeaker = true;
            }
            else
            {
                _continueButton.gameObject.SetActive(false);
                StartCoroutine(ExecuteCommandsWithDelay());
            }
        }

        private void ExecuteCommand(string command)
        {
            _commandExecutor.ExecuteCommand(command);
            DisplayNextPage(0.05f);
        }

        private IEnumerator ExecuteCommandsWithDelay()
        {
            yield return new WaitForSeconds(_commandExecutor.TransitionDelay);
            ExecuteCommands();
        }

        private void ExecuteCommands()
        {
            if (_dialogueService == null)
                return;

            var commands = _dialogueService.CurrentDialogue?.Commands;
            if (commands == null)
                return;

            foreach (var command in commands)
            {
                _commandExecutor.ExecuteCommand(command);
            }

            commands.Clear();
        }
    }
}
