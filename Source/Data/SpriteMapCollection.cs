using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [CollectionDataContract]
    public class SpriteMapCollection : List<SpriteMap>
    {
        #region Create
            public SpriteMap Create(string imageName) 
            {
                return (this.Create(imageName, string.Empty));
            }

            public SpriteMap Create(string imageName, string name) 
            {
                return (Create(imageName, name, string.Empty));
            }

            public SpriteMap Create(string imageName, string name, string transparency) 
            {
                SpriteMap spriteMap = new SpriteMap();
                spriteMap.ImageName = imageName;
                spriteMap.Name = name;
                spriteMap.Transparency = transparency;
                this.Add(spriteMap);
                return (spriteMap);
            }
        #endregion
        #region Get
            public SpriteMap GetSpriteMap(string name) 
            {
                foreach (SpriteMap spriteMap in this)
                    if (spriteMap.Name == name)
                        return (spriteMap);
                return (null);
            }
        #endregion
    }
}
