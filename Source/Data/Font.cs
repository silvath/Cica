using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Font
    {
        #region Properties
            [DataMember]
            public string Name { set; get;}
            [DataMember]
            public string ImageName { set; get; }
            [DataMember]
            public string Transparency { set; get; }
            [DataMember]
            public CharacterCollection Characters { set; get; }
        #endregion
        #region Constructors
            public Font() 
            {
                this.Characters = new CharacterCollection();
            }
        #endregion
    }
}
