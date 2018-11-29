using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class SpriteCollection : List<Sprite>
    {
        public Sprite Create(int x, int y, int xImage, int yImage, int width, int height) 
        {
            return (Create(x, y, xImage, yImage, width, height, true)); 
        } 

        public Sprite Create(int x, int y, int xImage, int yImage, int width, int height, bool transparency) 
        {
            Sprite sprite = new Sprite();
            sprite.X = x;
            sprite.Y = y;
            sprite.XImage = xImage;
            sprite.YImage = yImage;
            sprite.Width = width;
            sprite.Height = height;
            sprite.Transparency = transparency;
            this.Add(sprite);
            return (sprite);
        }
    }
}
