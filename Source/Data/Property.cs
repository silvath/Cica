using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Property
    {
        #region Constants
            public static string PROPERTY_WINNER = "WINNER";
        #endregion
        #region Properties
            [DataMember]
            public string AttributeName { set; get; }
            [DataMember]
            public string PropertyName { set; get; }
            [DataMember]
            public string Value { set; get; }
        #endregion

        #region Equal
            public bool IsEqual(Property property)
            {
                //AttributeName
                if (this.AttributeName != property.AttributeName)
                    return (false);
                //PropertyName
                if (this.PropertyName != property.PropertyName)
                    return (false);
                //Value
                if (this.Value != property.Value)
                    return (false);
                return (true);
            }
        #endregion
        #region Clone
            public Property Clone() 
            {
                Property clone = new Property();
                //AttributeName
                clone.AttributeName = this.AttributeName;
                //PropertyName
                clone.PropertyName = this.PropertyName;
                //Value
                clone.Value = this.Value;
                return (clone);
            }
        #endregion
    }
}
