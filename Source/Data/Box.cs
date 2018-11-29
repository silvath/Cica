using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Box
    {
        #region Properties
            [DataMember]
            public Rectangle Rectangle { set; get; }
            [DataMember]
            public Color Color { set; get; }
        #endregion
        #region Constructors
            public Box() 
            {
                this.Rectangle = new Rectangle();
                this.Color = new Color();
            }

            public Box(Rectangle rectangle, Color color) 
            {
                this.Rectangle = rectangle;
                this.Color = color;
            }
        #endregion

        #region Clone
            public Box Clone()
            {
                Box box = new Box();
                box.Rectangle = this.Rectangle.Clone();
                box.Color = this.Color.Clone();
                return(box);
            }
        #endregion
    }
}
