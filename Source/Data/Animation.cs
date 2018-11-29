using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Animation
    {
        #region Properties
            [DataMember]
            public string Name { set; get; }
            [DataMember]
            public bool Loop { set; get; }
            [DataMember]
            public FrameCollection Frames { set; get; }
        #endregion
        #region Constructors
            public Animation() 
            {
                this.Frames = new FrameCollection();
            }
        #endregion
    }
}
