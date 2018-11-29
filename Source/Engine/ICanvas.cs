using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public interface ICanvas
    {
        #region Create
            byte[] Create(int width, int height);        
        #endregion
        #region Draw
            byte[] Draw(byte[] data, byte[] image, int x, int y, int width, int height);        
        #endregion
        #region Extract
            byte[] Extract(Data.Image image, int x, int y, int width, int height, string transparency);
        #endregion
        #region Transform
            byte[] Transform(byte[] data);
        #endregion
    }
}
