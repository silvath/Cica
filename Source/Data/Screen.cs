using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Screen
    {
        #region Properties
            [DataMember]
            public ScreenType Type { set; get; }
            [DataMember]
            public string Name { set; get; }
            [DataMember]
            public string AnimationName { set; get; }
            [DataMember]
            public ActionCollection Actions { set; get; }
            [DataMember]
            public TextCollection Texts { set; get; }
            [DataMember]
            public TouchButtonCollection TouchButtons { set; get; }
        #endregion
        #region Constructors
            public Screen() 
            {
                this.Actions = new ActionCollection();
                this.Texts = new TextCollection();
                this.TouchButtons = new TouchButtonCollection();
            }
        #endregion
    }
}
