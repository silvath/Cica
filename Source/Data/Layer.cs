using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class Layer
    {
        #region Properties
            public int? Code { set; get; }
            public int X { set; get; }
            public int Y { set; get; }
            public int Width { set; get; }
            public int Height { set; get; }
            public byte[] Data { set; get; }
            public BoxCollection Boxes { set; get; }
        #endregion
        #region Constructors
            public Layer() 
            {
                this.Boxes = new BoxCollection();
            }

            public Layer Clone()
            {
                Layer clone = new Layer();
                clone.Code = Code;
                clone.X = this.X;
                clone.Y = this.Y;
                clone.Width = this.Width;
                clone.Height = this.Height;
                clone.Data = this.Data;
                clone.Boxes = this.Boxes;
                return (clone);
            }
        #endregion
    }
}
