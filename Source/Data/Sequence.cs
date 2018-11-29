using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class Sequence
    {
        #region Properties
            [DataMember]
            public List<List<InputType>> Inputs { set; get; } 
        #endregion
        #region Constructors
            public Sequence() 
            {
                this.Inputs = new List<List<InputType>>();
            }
        #endregion

        #region Add
            public void Add(InputType input) 
            {
                List<InputType> inputs = new List<InputType>();
                inputs.Add(input);
                this.Add(inputs);
            }

            public void Add(List<InputType> inputs)
            {
                this.Inputs.Add(inputs);
            }
        #endregion
    }
}
