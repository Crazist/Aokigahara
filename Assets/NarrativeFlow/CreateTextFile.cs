using UnityEditor;
using UnityEngine;
using System.IO;

namespace NarrativeFlow
{
    public class CreateNarrativeFile
    {
        [MenuItem("Assets/Create/Narrative Flow/Dialog")]
        public static void CreateNewTextFile()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }
            else if (!AssetDatabase.IsValidFolder(path))
            {
                path = Path.GetDirectoryName(path);
            }

            string fileName = "NewTextFile.narr";
            string fullPath = Path.Combine(path, fileName);
            int fileIndex = 1;

            while (File.Exists(fullPath))
            {
                fileName = $"NewTextFile{fileIndex}.narr";
                fullPath = Path.Combine(path, fileName);
                fileIndex++;
            }

            File.WriteAllText(fullPath, GetDefaultContent());
            AssetDatabase.Refresh();

            Object asset = AssetDatabase.LoadAssetAtPath<Object>(fullPath);
            Selection.activeObject = asset;
        }

        private static string GetDefaultContent()
        {
            return @"# Текст диалога
Это пример текста диалога.

# Команды
#change_background forest_day
#play_music calm_theme";
        }
    }
}