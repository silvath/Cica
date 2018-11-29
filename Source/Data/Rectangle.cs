using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Rectangle
    {
        #region Properties
            [DataMember]
            public int X { set; get; }
            [DataMember]
            public int Y { set; get; }
            [DataMember]
            public int Width { set; get; }
            [DataMember]
            public int Height { set; get; }
        #endregion
        #region Constructors
            public Rectangle() 
            {
            }

            public Rectangle(int x, int y, int width, int height) 
            {
                this.X = x;
                this.Y = y;
                this.Width = width;
                this.Height = height;
            }
        #endregion

        #region Clone
            public Rectangle Clone() 
            {
                return (new Rectangle(this.X, this.Y, this.Width, this.Height));
            }
        #endregion
    }
}
