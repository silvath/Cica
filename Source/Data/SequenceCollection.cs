using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class SequenceCollection : List<Sequence>
    {
        public Sequence Create() 
        {
            Sequence sequence = new Sequence();
            this.Add(sequence);
            return (sequence);
        }
    }
}
