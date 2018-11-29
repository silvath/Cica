using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Action
    {
        #region Properties
            [DataMember]
            public ActionType Type { set; get; }
            [DataMember]
            public InputType? Input { set; get; }
            [DataMember]
            public Rectangle Touch { set; get; }
            [DataMember]
            public string ScreenName { set; get; }
        #endregion
        #region Constructors
            public Action() 
            {

            }
        #endregion
    }
}
