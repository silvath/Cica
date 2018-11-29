using Engine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Bitmap = SharpDX.WIC.Bitmap;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace EngineWindowsUniversal
{
    public class CanvasWindowsUniversal : ICanvas
    {
        #region Attributes
        #endregion
        #region Properties
        private Dictionary<int, List<int>> _data = new Dictionary<int, List<int>>();
        #endregion
        #region Constructors
        public CanvasWindowsUniversal()
        {
        }
        #endregion
        #region Create
        byte[] ICanvas.Create(int width, int height)
        {
            //TODO: Work over here
            return (null);
        }
        #endregion
        #region Draw
        byte[] ICanvas.Draw(byte[] data, byte[] image, int x, int y, int width, int height)
        {
            //TODO: Work over here
            return (null);
        }

        private bool HasData(int x, int y)
        {
            //TODO: Work over here
            return (false);
        }

        private void AddData(int x, int y)
        {
            //TODO: Work over here
        }
        #endregion
        #region Extract
        byte[] ICanvas.Extract(Data.Image image, int x, int y, int width, int height, string transparency)
        {
            //TODO: Work over here
            return (null);
        }

        private byte GetByteFromHexa(string hexa)
        {
            return (Byte.Parse(hexa, NumberStyles.HexNumber));
        }
        #endregion
        #region Transform
        byte[] ICanvas.Transform(byte[] data)
        {
            //TODO: Work over here
            return (null);
        }
        #endregion
    }
}
