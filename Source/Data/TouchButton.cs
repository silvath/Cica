using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class TouchButton
    {
        #region Properties
            [DataMember]
            public int? Top { set; get; }
            [DataMember]
            public int? Down { set; get; }
            [DataMember]
            public int? Left { set; get; }
            [DataMember]
            public int? Right { set; get; }
            [DataMember]
            public int Width { set; get; }
            [DataMember]
            public int Height { set; get; }
            [DataMember]
            public InputType Input { set; get; }
            [DataMember]
            public string AnimationName { set; get; }
        #endregion
        #region Constructors
            public TouchButton() 
            {
            }
        #endregion
    }
}
