using System.Collections.Generic;

namespace NarrativeFlow
{
    public class DialogContent
    {
        public List<string> Commands { get; set; }
        public string Dialogue { get; set; }
        public float DisplaySpeed { get; set; }

        public DialogContent() => 
            Commands = new List<string>();
    }
}