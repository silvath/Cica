using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Frame
    {
        #region Properties
            [DataMember]
            public string SpriteMapName { set; get; }
            [DataMember]
            public int Frames { set; get; }
            [DataMember]
            public int X { set; get; }
            [DataMember]
            public int Y { set; get; }
            [DataMember]
            public HitBoxCollection HitBoxesDamage { set; get; }
            [DataMember]
            public HitBoxCollection HitBoxesVulnerable { set; get; }
        #endregion
        #region Constructors
            public Frame() 
            {
                this.HitBoxesDamage = new HitBoxCollection();
                this.HitBoxesVulnerable = new HitBoxCollection();
            }
        #endregion
    }
}
