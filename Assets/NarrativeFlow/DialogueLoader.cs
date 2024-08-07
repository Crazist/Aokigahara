using System.Collections.Generic;
using System.IO;

namespace NarrativeFlow
{
    public class DialogueLoader
    {
        public Dictionary<string, DialogContent> Dialogues { get;}

        public DialogueLoader(string directoryPath)
        {
            Dialogues = new Dictionary<string, DialogContent>();
            LoadAllDialogues(directoryPath);
        }

        private void LoadAllDialogues(string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.narr", SearchOption.AllDirectories))
            {
                string content = File.ReadAllText(file);
                DialogContent loadedDialogue = ParseTextFile(content);
                Dialogues.Add(Path.GetFileNameWithoutExtension(file), loadedDialogue);
            }
        }

        private DialogContent ParseTextFile(string content)
        {
            DialogContent textFileContent = new DialogContent();
            textFileContent.Commands = new List<string>();
            textFileContent.Dialogue = "";

            using (StringReader reader = new StringReader(content))
            {
                string line;
                bool readingDialogue = false;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("# Текст диалога"))
                    {
                        readingDialogue = true;
                        continue;
                    }
                    else if (readingDialogue)
                    {
                        textFileContent.Dialogue += line + "\n";
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
