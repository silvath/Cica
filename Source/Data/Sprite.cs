using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Sprite
    {
        #region Properties
            [DataMember]
            public int X { set; get; }
            [DataMember]
            public int Y { set; get; }
            [DataMember]
            public int XImage { set; get; }
            [DataMember]
            public int YImage { set; get; }
            [DataMember]
            public int Width { set; get; }
            [DataMember]
            public int Height { set; get; }
            [DataMember]
            public bool Transparency { set; get; }
            public string Name 
            {
                get 
                {
                    return (string.Format("XI={0},YI={1},W={2},H={3},X={4},Y={5}", this.XImage, this.YImage, this.Width, this.Height, this.X, this.Y));
                }
            }
        #endregion
    }
}
