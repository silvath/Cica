using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Trigger
    {
        #region Properties
            [DataMember]
            public TriggerType Type { set; get; }
            [DataMember]
            public string ResourceName { set; get; }
            [DataMember]
            public string AnimationName { set; get; }
            [DataMember]
            public IntRandom Activation { set; get; }
            [DataMember]
            public IntRandom Life { set; get; }
            [DataMember]
            public IntRandom X { set; get; }
            [DataMember]
            public IntRandom Y { set; get; }
            [DataMember]
            public IntRandom VelocityX { set; get; }
            [DataMember]
            public IntRandom VelocityY { set; get; }
            public string Name 
            {
                get 
                {
                    return (string.Format("{0} - {1}", this.AnimationName, this.Activation));
                }
            }
        #endregion
        #region Constructor
            public Trigger() 
            {
                this.Activation = new IntRandom(0, 0);
                this.Life = new IntRandom(0, 0);
                this.X = new IntRandom(0, 0);
                this.Y = new IntRandom(0, 0);
                this.VelocityX = new IntRandom(0, 0);
                this.VelocityY = new IntRandom(0, 0);
            }
        #endregion
    }
}
