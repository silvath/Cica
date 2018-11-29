using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class CacheManager
    {
        #region Properties
            private Dictionary<string, SpriteMap> _cache = new Dictionary<string, SpriteMap>();
        #endregion
        #region Constructor
            public CacheManager() 
            {

            }
        #endregion

        #region Key
            public string CreateKey(Text text, string value) 
            {
                return (string.Format("F{0}V{1}", text.FontName, value));
            }
        #endregion
        #region Contains
            public bool Contains(string key) 
            {
                return (this._cache.ContainsKey(key));
            }
        #endregion
        #region Add
            public void Add(string key, SpriteMap spriteMap) 
            {
                this._cache.Add(key, spriteMap);
            }
        #endregion
        #region Get
            public SpriteMap Get(string key) 
            {
                return (this._cache[key]);
            }
        #endregion
    }
}
