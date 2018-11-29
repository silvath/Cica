using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class FontCollection : List<Font>
    {
        public Font Create(string name, string imageName, string transparency) 
        {
            Font font = new Font();
            font.Name = name;
            font.ImageName = imageName;
            font.Transparency = transparency;
            this.Add(font);
            return (font);
        }

        public Font GetFont(string name) 
        {
            foreach (Font font in this)
                if (font.Name == name)
                    return (font);
            return (null);
        }
    }
}
