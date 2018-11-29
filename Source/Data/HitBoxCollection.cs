using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class HitBoxCollection : List<HitBox>
    {
        public HitBox Create(int x, int y, int width, int height) 
        {
            HitBox hitbox = new HitBox();
            hitbox.X = x;
            hitbox.Y = y;
            hitbox.Width = width;
            hitbox.Height = height;
            this.Add(hitbox);
            return (hitbox);
        } 
    }
}
