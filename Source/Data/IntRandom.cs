using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class IntRandom
    {
        #region Properties
            [DataMember]
            public int Start { set; get; }
            [DataMember]
            public int? End { set; get; }
        public int Value
        {
            get
            {
                if (!this.End.HasValue)
                    return (this.Start);
                Random random = new Random(DateTime.Now.Millisecond);
                return (random.Next(this.Start, this.End.Value));
            }
        }
        #endregion
        #region Constructors
            public IntRandom(int start) 
            {
                this.Start = start;
            }
            public IntRandom(int start, int end)
            {
                this.Start = start;
                this.End = end;
            }

            public IntRandom(int start, int? end)
            {
                this.Start = start;
                this.End = end;
            }

            public override string ToString()
            {
                return (string.Format("A({0}-{1})", this.Start, this.End ?? 0)); 
            }
        #endregion
    }
}
