using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class TextCollection : List<Text>
    {
        public Text Create(string fontName, string value, int x, int y) 
        {
            return (Create(fontName, value, x, y, TextAlign.Left)); 
        } 

        public Text Create(string fontName, string value, int x, int y, TextAlign align) 
        {
            Text text = new Text();
            text.FontName = fontName;
            text.Value = value;
            text.X = x;
            text.Y = y;
            text.Align = align;
            this.Add(text);
            return (text);
        }

        public bool IsEqual(TextCollection texts) 
        {
            if (this.Count != texts.Count)
                return(false);
            for (int i = 0; i < this.Count; i++)
                if (!this[i].IsEqual(texts[i]))
                    return (false);
            return (true);
        }

        public TextCollection Clone()
        {
            TextCollection clone = new TextCollection();
            foreach (Text text in this)
                clone.Add(text.Clone());
            return (clone);
        }
    }
}

