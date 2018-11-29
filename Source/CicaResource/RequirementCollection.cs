using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaResource
{
    public class RequirementCollection : List<Requirement>
    {
        #region Propeties
        #endregion
        #region Constructors
            public RequirementCollection() 
            {
            }
        #endregion

        #region Create
            public Requirement Create(Resource resource) 
            {
                Requirement requirement = new Requirement();
                requirement.ResourceCode = resource.Code;
                this.Add(requirement);
                return(requirement);
            }
        #endregion
    }
}
