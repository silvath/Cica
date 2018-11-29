using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Text
    {
        [DataMember]
        public string FontName { set; get; }
        [DataMember]
        public string Value { set; get; }
        [DataMember]
        public int X { set; get; }
        [DataMember]
        public int Y { set; get; }
        [DataMember]
        public TextAlign Align { set; get; }

        public bool IsEqual(Text text) 
        {
            //FontName
            if (this.FontName != text.FontName)
                return (false);
            //Value
            if (this.Value != text.Value)
                return (false);
            //X
            if (this.X != text.X)
                return (false);
            //Y
            if (this.Y != text.Y)
                return (false);
            //Align
            if (this.Align != text.Align)
                return (false);
            return (true);
        }

        public Text Clone() 
        {
            Text clone = new Text();
            //FontName
            clone.FontName = this.FontName;
            //Value
            clone.Value = this.Value;
            //X
            clone.X = this.X;
            //Y
            clone.Y = this.Y;
            //Align
            clone.Align = this.Align;
            return(clone);
        }
    }
}
