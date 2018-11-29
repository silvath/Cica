using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Character
    {
        [DataMember]
        public string Name { set; get; }
        [DataMember]
        public int X { set; get; }
        [DataMember]
        public int Y { set; get; }
        [DataMember]
        public int Width { set; get; }
        [DataMember]
        public int Height { set; get; }
    }
}
