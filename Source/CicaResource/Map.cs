using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaResource
{
    [DataContract]
    public class Map : Resource
    {
        internal override ResourceType GetResourceType()
        {
            return (ResourceType.Map);
        }
    }
}
