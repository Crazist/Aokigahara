using System;
using System.Collections.Generic;
using UnityEngine;

namespace NarrativeFlow
{
    public class TextSplitter
    {
        public IEnumerable<string> SplitTextIntoPages(string text, int maxCharactersPerPage)
        {
            List<string> pages = new List<string>();
            int currentIndex = 0;

            while (currentIndex < text.Length)
            {
                if (text[currentIndex] == '#')
                {
                    int endCommandIndex = text.IndexOf('\n', currentIndex);
                    if (endCommandIndex == -1)
                    {
                        endCommandIndex = text.Length;
                    }

                    string command = text.Substring(currentIndex, endCommandIndex - currentIndex).Trim();
                    pages.Add(command);
                    currentIndex = endCommandIndex + 1;
                    continue;
                }

                int length = Mathf.Min(maxCharactersPerPage, text.Length - currentIndex);
                string pageText = text.Substring(currentIndex, length);

                int nextParagraph = text.IndexOf("\n\n", currentIndex, StringComparison.Ordinal);
                if (nextParagraph != -1 && nextParagraph < currentIndex + length)
                {
                    length = nextParagraph - currentIndex;
                    pageText = text.Substring(currentIndex, length);
                    currentIndex = nextParagraph + 2;
                }
                else
                {
                    if (currentIndex + length < text.Length && !char.IsWhiteSpace(text[currentIndex + length]))
                    {
                        int lastSpace = pageText.LastIndexOf(' ');
                        if (lastSpace != -1)
                        {
                            length = lastSpace;
                            pageText = text.Substring(currentIndex, length);
                        }
                    }
                    currentIndex += length;
                }

                if (!string.IsNullOrWhiteSpace(pageText.TrimEnd()))
                {
                    pages.Add(pageText.TrimEnd());
                }
            }

            return pages;
        }
    }
}
