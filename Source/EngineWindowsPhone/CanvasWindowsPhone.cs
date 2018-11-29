using Engine;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Bitmap = SharpDX.WIC.Bitmap;
using PixelFormat = SharpDX.WIC.PixelFormat;

namespace EngineWindowsPhone
{
    public class CanvasWindowsPhone : ICanvas
    {
        #region Attributes
        #endregion
        #region Properties
            private Dictionary<int, List<int>> _data = new Dictionary<int,List<int>>();
        #endregion
        #region Constructors
            public CanvasWindowsPhone() 
            {
            }
        #endregion
        #region Create
            byte[] ICanvas.Create(int width, int height) 
            {
                this._data = new Dictionary<int, List<int>>();
                Image image = CreateImage(width, height);
                MemoryStream memo = new MemoryStream();
                image.Save(memo, ImageFileType.Png);
                image.Dispose();
                return (memo.ToArray());
            }
        #endregion
        #region Draw
            byte[] ICanvas.Draw(byte[] data, byte[] image, int x, int y, int width, int height)
            {
                Image imageBase = Image.Load(data.ToArray());
                Image imageDraw = Image.Load(image.ToArray());
                PixelBuffer bufferBase = imageBase.PixelBuffer[0];
                PixelBuffer bufferDraw = imageDraw.PixelBuffer[0];
                for (int iy = 0; iy < height; iy++)
                {
                    for (int ix = 0; ix < width; ix++)
                    {
                        byte[] bytes = BitConverter.GetBytes(bufferDraw.GetPixel<uint>(ix, iy));
                        if ((bytes[3] == 0) && (HasData(x + ix, y + iy)))
                            continue;
                        bufferBase.SetPixel<uint>(x + ix, y + iy, BitConverter.ToUInt32(bytes, 0));
                        AddData(x + ix, y + iy);
                    }
                }
                MemoryStream memo = new MemoryStream();
                imageBase.Save(memo, ImageFileType.Png);
                imageDraw.Dispose();
                imageBase.Dispose();
                return (memo.ToArray());
            }

            private bool HasData(int x, int y) 
            {
                if (!this._data.ContainsKey(x))
                    return (false);
                return (this._data[x].Contains(y));
            }

            private void AddData(int x, int y) 
            {
                if (!this._data.ContainsKey(x))
                    this._data.Add(x, new List<int>());
                this._data[x].Add(y);
            }
        #endregion
        #region Extract
            byte[] ICanvas.Extract(Data.Image image, int x, int y, int width, int height, string transparency)
            {
                Image imageSource = Image.Load(image.Data.ToArray());
                Image imageNew = CreateImage(width, height);
                PixelBuffer bufferSource = imageSource.PixelBuffer[0];
                PixelBuffer bufferNew = imageNew.PixelBuffer[0];
                byte? tR = string.IsNullOrEmpty(transparency) ? null : (byte?)GetByteFromHexa(transparency.Substring(0, 2));
                byte? tG = string.IsNullOrEmpty(transparency) ? null : (byte?)GetByteFromHexa(transparency.Substring(2, 2));
                byte? tB = string.IsNullOrEmpty(transparency) ? null : (byte?)GetByteFromHexa(transparency.Substring(4, 2));
                for (int iy = y; iy < (y + height); iy++)
                {
                    for (int ix = x; ix < (x + width); ix++)
                    {
                        byte[] bytes = BitConverter.GetBytes(bufferSource.GetPixel<uint>(ix, iy));
                        if (tR.HasValue) 
                        {
                            if ((bytes[0] == tR.Value) && (bytes[1] == tG.Value) && (bytes[2] == tB.Value))
                            {
                                bytes[0] = 0;
                                bytes[1] = 0;
                                bytes[2] = 0;
                                bytes[3] = 0;
                            }
                        }
                        bufferNew.SetPixel<uint>(ix - x, iy - y, BitConverter.ToUInt32(bytes, 0));
                    }
                }
                imageSource.Dispose();
                MemoryStream memo = new MemoryStream();
                imageNew.Save(memo, ImageFileType.Png);
                imageNew.Dispose();
                return (memo.ToArray());
            }

            private byte GetByteFromHexa(string hexa)
            {
                return (Byte.Parse(hexa, NumberStyles.HexNumber));
            }
        #endregion
        #region Transform
            byte[] ICanvas.Transform(byte[] data) 
            {
                List<byte> dataNew = new List<byte>();
                Image bitmap = Image.Load(data.ToArray());
                PixelBuffer buffer = bitmap.PixelBuffer[0];
                for (int y = 0; y < buffer.Height; y++)
                {
                    for (int x = 0; x < buffer.Width; x++)
                    {
                        byte[] bytes = BitConverter.GetBytes(buffer.GetPixel<uint>(x, y));
                        dataNew.Add(bytes[0]);
                        dataNew.Add(bytes[1]);
                        dataNew.Add(bytes[2]);
                        dataNew.Add(bytes[3]);
                    }
                }
                bitmap.Dispose();
                return (dataNew.ToArray());
            }
        #endregion

        #region Toolkit
            public Image CreateImage(int width, int height) 
            {
                ImageDescription imageDescription = new ImageDescription();
                imageDescription.Width = width;
                imageDescription.Height = height;
                imageDescription.Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm;
                imageDescription.Dimension = TextureDimension.Texture2D;
                imageDescription.Depth = 1;
                imageDescription.ArraySize = 1;
                imageDescription.MipLevels = 1;
                return(Image.New(imageDescription));
            }
        #endregion

    }
}
