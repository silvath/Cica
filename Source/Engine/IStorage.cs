using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public interface IStorage
    {
        #region Read
            Dictionary<string, byte[]> ReadAll(bool loadData);
        #endregion
        #region Write
            void Write(string name, byte[] data);
        #endregion
        #region Delete
            void Delete(string name);
        #endregion
    }
}
