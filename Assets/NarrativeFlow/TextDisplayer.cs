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
            _pages = new Queue<string>(_textSplitter.SplitTextIntoPages(dialogContent.Dialogue, _maxCharactersPerPage));
            DisplayNextPage(dialogContent.DisplaySpeed);
        }

        private void OnContinueButtonClicked()
        {
            if (_isWaitingForInput)
            {
                _isWaitingForInput = false;
                _continueButton.gameObject.SetActive(false);
                DisplayNextPage(0.05f);
            }
        }

        private void DisplayNextPage(float speed)
        {
            if (_pages.Count > 0)
            {
                string page = _pages.Dequeue();
                StartCoroutine(TypeText(page, speed));
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

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '#' && (i == 0 || text[i - 1] == '\n'))
                {
                    string command = ExtractCommand(ref i, text);
                    _commandExecutor.ExecuteCommand(command);
                    _isWaitingForInput = true;
                    OnContinueButtonClicked();
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
            }
            else
            {
                _continueButton.gameObject.SetActive(false);
                StartCoroutine(ExecuteCommandsWithDelay());
            }
        }

        private string ExtractCommand(ref int index, string text)
        {
            int endIndex = text.IndexOf('\n', index);
            if (endIndex == -1)
            {
                endIndex = text.Length;
            }

            string command = text.Substring(index, endIndex - index).Trim();
            index = endIndex;
            return command;
        }

        private IEnumerator ExecuteCommandsWithDelay()
        {
            yield return new WaitForSeconds(_commandExecutor.TransitionDelay);
            ExecuteCommands();
        }

        public void ExecuteCommands()
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