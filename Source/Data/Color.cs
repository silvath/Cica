using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Color
    {
        #region Properties
            [DataMember]
            public int Red { set; get; }
            [DataMember]
            public int Green { set; get; }
            [DataMember]
            public int Blue { set; get; }
            [DataMember]
            public float Alpha { set; get; }
        #endregion
        #region Constructor
            public Color() 
            {
            }

            public Color(int red, int green, int blue, float alpha)
            {
                this.Red = red;
                this.Green = green;
                this.Blue = blue;
                this.Alpha = alpha;
            }
        #endregion

        #region Clone
            public Color Clone() 
            {
                return (new Color(this.Red, this.Green, this.Blue, this.Alpha));
            }
        #endregion
    }
}
