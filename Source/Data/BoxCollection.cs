using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class BoxCollection : List<Box>
    {
        #region Create
        public Box Create(int x, int y, int width, int height, int red, int green, int blue, float alpha)
        {
            Box box = new Box(new Rectangle(x, y, width, height), new Color(red, green, blue, alpha));
            this.Add(box);
            return (box);
        }
        #endregion

        #region Clone
        public BoxCollection Clone() 
        {
            BoxCollection clone = new BoxCollection();
            foreach(Box box in this)
                clone.Add(box.Clone());
            return (clone);
        }
        #endregion
    }
}
