using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Attribute
    {
        #region Properties
            [DataMember]
            public string Name { set; get; }
            [DataMember]
            public AttributeCategory Category { set; get; }
        #endregion
    }
}
