using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Command
    {
        #region Properties
            [DataMember]
            public string Name { set; get; }
            [DataMember]
            public string FactoryName { set; get; }
            [DataMember]
            public string AnimationName { set; get; }
            [DataMember]
            public string AnimationNamePrerequisite { set; get; }
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
            public SequenceCollection Sequences { set; get; }
        #endregion
        #region Constructors
            public Command() 
            {
                this.Sequences = new SequenceCollection();
            }
        #endregion
    }
}
