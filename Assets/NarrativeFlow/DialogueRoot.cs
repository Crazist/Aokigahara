using System.IO;
using UnityEditor;
using UnityEngine;

namespace NarrativeFlow
{
    [CreateAssetMenu(fileName = "DialogueRoot", menuName = "Narrative Flow/Dialogue Root")]
    public class DialogueRoot : ScriptableObject
    {
        private DialogueLoader _dialogueLoader;

        public void Initialize()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string directory = Path.GetDirectoryName(path);

            _dialogueLoader = new DialogueLoader(directory);
        }

        public DialogContent GetDialogue(string dialogueName)
        {
            if (_dialogueLoader.Dialogues.TryGetValue(dialogueName, out DialogContent dialogue))
            {
                return dialogue;
            }
            else
            {
                Debug.LogError("Dialogue not found: " + dialogueName);
                return null;
            }
        }
    }
}