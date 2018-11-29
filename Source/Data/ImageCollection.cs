using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class ImageCollection : List<Image>
    {
        public Image Create(string name, int width, int height, List<byte> data) 
        {
            Image image = new Image();
            image.Name = name;
            image.Width = width;
            image.Height = height;
            image.Data = data;
            this.Add(image);
            return (image);
        }

        public Image GetImage(string imageName) 
        {
            foreach (Image image in this)
                if (image.Name == imageName)
                    return (image);
            return (null);
        }
    }
}
