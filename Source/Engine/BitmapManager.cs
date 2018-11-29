using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class BitmapManager<T> where T : IDisposable
    {
        #region Attributes
            private Dictionary<int, T> _cache = new Dictionary<int, T>();
            private List<int> _used = new List<int>();
        #endregion
        #region Constructors
            public BitmapManager() 
            {

            }
        #endregion

        #region Get
            public T GetBitmap(int code) 
            {
                if (!this._cache.ContainsKey(code))
                    return(default(T));
                if (!this._used.Contains(code))
                    this._used.Add(code);
                return(this._cache[code]);
            }
        #endregion
        #region Contains
            public bool Contains(int code) 
            {
                return(this._cache.ContainsKey(code));
            }
        #endregion

        #region Add
            public void Add(int code, T value) 
            {
                this._cache.Add(code, value);
            }
        #endregion

        #region Start
            public void Start() 
            {
                _used.Clear();
            }
        #endregion
        #region End
            public void End()
            {
                List<int> discards = new List<int>();
                foreach (KeyValuePair<int, T> entry in _cache) 
                    if (!this._used.Contains(entry.Key))
                        discards.Add(entry.Key);
                foreach (int discard in discards) 
                {
                    this._cache[discard].Dispose();
                    this._cache.Remove(discard);
                }
            }
        #endregion
    }
}
