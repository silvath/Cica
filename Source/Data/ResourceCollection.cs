using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class ResourceCollection : List<Resource>
    {
        #region Create
            public Resource Create(Resource parent, ResourceType type, string code, string name)
            {
                Resource resource = new Resource(parent);
                resource.Type = type;
                resource.Code = code;
                resource.Name = name;
                this.Add(resource);
                return (resource);
            }
        #endregion

        #region Search
            public Resource GetResource(string resourceName)
            {
                foreach (Resource resource in this)
                    if (resource.Name == resourceName)
                        return (resource);
                return (null);
            }

            public Resource GetResourceByStorageID(string storageID)
            {
                foreach (Resource resource in this)
                    if (resource.StorageID == storageID)
                        return (resource);
                return (null);
            }

            public Resource GetResourceByCode(string code) 
            {
                foreach (Resource resource in this)
                {
                    if (resource.Code.ToString() == code)
                        return (resource);
                    Resource resourceChild = resource.Resources.GetResourceByCode(code);
                    if (resourceChild != null)
                        return (resourceChild);
                }
                return (null);
            }
        #endregion
    }
}
