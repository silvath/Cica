using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Spawn
    {
        #region Properties
            [DataMember]
            public int X { set; get; }
            [DataMember]
            public int Y { set; get; }
            [DataMember]
            public int VelocityX { set; get; }
            [DataMember]
            public int VelocityY { set; get; }
            [DataMember]
            public int Life { set; get; }
            [DataMember]
            public string AnimationName { set; get; }
            [DataMember]
            public string GroupName { set; get; }
            public string Name 
            {
                get 
                {
                    return (string.Format("{0} - {1} - {2} - {3}", this.GroupName, this.AnimationName, this.X, this.Y));
                }
            }
        #endregion
    }
}
