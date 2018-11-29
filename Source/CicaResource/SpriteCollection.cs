using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cica.CicaResource
{
    public class SpriteCollection : List<Sprite>
    {
        #region Propeties
        #endregion
        #region Constructors
            public SpriteCollection() 
            {
            }
        #endregion

        #region Create
            public int Create(List<byte> data) 
            {
                int code = 1;
                while (this.Find(s => s.Code == code) != null)
                    code++;
                Sprite sprite = new Sprite();
                sprite.Code = code;
                sprite.Data = data;
                this.Add(sprite);
                return (sprite.Code);
            }
        #endregion
    }
}
