using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaResource
{
    [DataContract]
    public abstract class Resource
    {
        #region Properties
            [DataMember]
            public Guid Code { set; get; }
            [DataMember]
            public string Name { set; get; }
            [DataMember]
            public RequirementCollection Requirements { set; get; }
            public string FullName 
            {
                get 
                {
                    return (string.Format("{0} - {1}", this.Code, this.Name));
                }
            }

            public string FullNameWithExtension
            {
                get
                {
                    return (string.Format("{0}{1}", this.FullName, this.GetResourceExtension()));
                }
            }

            [DataMember]
            public SpriteCollection Sprites { set; get; }
        #endregion
        #region Constructors
            public Resource() 
            {
                this.Code = Guid.NewGuid();
                this.Sprites = new SpriteCollection();
                this.Requirements = new RequirementCollection();
            }
        #endregion

        #region Type
            internal abstract ResourceType GetResourceType();

            public static ResourceType GetResourceTypeByFileName(string fileName)
            {
                string extension = Path.GetExtension(fileName).ToLower();
                if (extension == ".cac")
                    return (ResourceType.Actor);
                else if (extension == ".cma")
                    return (ResourceType.Map);
                else if (extension == ".cga")
                    return (ResourceType.Game);
                return (ResourceType.Unknown);
            }

            public static string GetResourceTypeExtension(ResourceType type) 
            {
                if (type == ResourceType.Actor)
                    return (".cac");
                else if (type == ResourceType.Map)
                    return (".cma");
                else if (type == ResourceType.Game)
                    return (".cga");
                return (string.Empty);
            }

            public string GetResourceExtension() 
            {
                return(GetResourceTypeExtension(this.GetResourceType()));
            }

            public static Type GetResourceTypeByExtension(string fileName) 
            {
                string extension = Path.GetExtension(fileName).ToLower();
                if (extension == ".cac")
                    return (typeof(Actor));
                else if (extension == ".cma")
                    return (typeof(Map));
                else if (extension == ".cga")
                    return (typeof(Game));
                return (null);
            }
        #endregion
    }
}
