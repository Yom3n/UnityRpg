using System;
using System.Collections.Generic;

namespace Dialog
{
    public class DialogData
    {
        public DialogData(string text, List<string> tags)
        {
            this.text = text;
            this.tags = tags;
        }

        public String text;
        public List<String> tags;
    }
}