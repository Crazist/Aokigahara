using System.Collections.Generic;
using System.IO;

namespace NarrativeFlow
{
    public class DialogueLoader
    {
        public Dictionary<string, DialogContent> dialogues;

        public DialogueLoader(string directoryPath)
        {
            dialogues = new Dictionary<string, DialogContent>();
            LoadAllDialogues(directoryPath);
        }

        private void LoadAllDialogues(string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.narr", SearchOption.AllDirectories))
            {
                string content = File.ReadAllText(file);
                DialogContent loadedDialogue = ParseTextFile(content);
                dialogues.Add(Path.GetFileName(file), loadedDialogue);
            }
        }

        private DialogContent ParseTextFile(string content)
        {
            DialogContent textFileContent = new DialogContent();
            textFileContent.Commands = new List<string>();

            using (StringReader reader = new StringReader(content))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("# Текст диалога"))
                    {
                        textFileContent.Dialogue = reader.ReadLine();
                    }
                    else if (line.StartsWith("#"))
                    {
                        textFileContent.Commands.Add(line.Substring(1).Trim());
                    }
                }
            }

            return textFileContent;
        }
    }
}