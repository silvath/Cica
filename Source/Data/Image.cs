using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Image
    {
        #region Properties
            [DataMember]
            public string Name { set; get; }
            [DataMember]
            public int Width { set; get; }
            [DataMember]
            public int Height { set; get; }
            [DataMember]
            public List<byte> Data { set; get; }
        #endregion
    }
}
