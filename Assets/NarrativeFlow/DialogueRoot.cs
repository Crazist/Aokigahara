using System.IO;
using UnityEditor;
using UnityEngine;

namespace NarrativeFlow
{
    [CreateAssetMenu(fileName = "DialogueRoot", menuName = "Narrative Flow/Dialogue Root")]
    public class DialogueRoot : ScriptableObject
    {
        private DialogueLoader dialogueLoader;

        public void Initialize()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string directory = Path.GetDirectoryName(path);

            dialogueLoader = new DialogueLoader(directory);
        }

        public DialogContent GetDialogue(string dialogueName)
        {
            if (dialogueLoader.dialogues.TryGetValue(dialogueName, out DialogContent dialogue))
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