using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaResource
{
    public class ResourceCollection : List<Resource>
    {
        #region Properties
            public List<string> Synchronized { set; get; }
            public bool IsReady
            {
                get
                {
                    return ((this.Count == this.Synchronized.Count) && (this.FindAll(r => r != null).Count == this.Count));
                }
            }
        #endregion
        #region Insert
            public void Insert(Resource resource) 
            {
                int index = this.Synchronized.IndexOf(resource.FullNameWithExtension);
                while (index > (this.Count - 1))
                    this.Add(null);
                this[index] = resource;
            }
        #endregion
    }
}
