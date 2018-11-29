using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Data
{
    [DataContract]
    public class SpriteMap
    {
        #region Attributes
            private List<Tuple<int, int, int, int, int , byte[]>> _cacheSprites = new List<Tuple<int, int, int, int, int, byte[]>>();
            private int _width = 0;
            private int _heigth = 0;
        #endregion
        #region Properties
            [DataMember]
            public int Code { set; get; }
            [DataMember]
            public string Name { set; get; }
            [DataMember]
            public string ImageName { set; get; }
            [DataMember]
            public string Transparency { set; get; }
            [DataMember]
            public SpriteCollection Sprites { set; get; }
            public List<Layer> CacheLayers { set; get; }
            private List<Tuple<int, int, int, int, int, byte[]>> CacheSprites 
            {
                get 
                {
                    if (this._cacheSprites == null)
                        this._cacheSprites = new List<Tuple<int, int, int, int, int, byte[]>>();
                    return (this._cacheSprites);
                }
            }

            public int Width 
            {
                set
                {
                    this._width = value;
                }
                get 
                {
                    if (this._width == 0) 
                    {
                        this._width = 1;
                        foreach (Sprite sprite in this.Sprites) 
                            if (this._width < (sprite.X + sprite.Width))
                                this._width = (sprite.X + sprite.Width);
                    }
                    return (this._width);
                }
            }

            public int Heigth 
            {
                set 
                {
                    this._heigth = value;
                }
                get 
                {
                    if (this._heigth == 0)
                    {
                        this._heigth = 1;
                        foreach (Sprite sprite in this.Sprites)
                            if (this._heigth < (sprite.Y + sprite.Height))
                                this._heigth = (sprite.Y + sprite.Height);
                    }
                    return (this._heigth);
                }
            }
        #endregion
        #region Constructors
            public SpriteMap() 
            {
                this.Sprites = new SpriteCollection();
            }
        #endregion

        #region Cache
            public byte[] GetCacheSprite(int x, int y, int width, int height, out int code) 
            {
                code = 0;
                foreach (Tuple<int, int, int, int, int, byte[]> item in this.CacheSprites)
                {
                    if ((item.Item1 == x) && (item.Item2 == y) && (item.Item3 == width) && (item.Item4 == height))
                    {
                        code = item.Item5;
                        return (item.Item6);
                    }
                }
                return (null);
            }

            public void SetCacheSprite(int code, int x, int y, int width, int height, byte[] data)
            {
                this.CacheSprites.Add(new Tuple<int, int, int, int, int, byte[]>(x, y, width, height, code, data));
            }
        #endregion
    }
}
