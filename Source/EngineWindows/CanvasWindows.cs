using Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EngineWindows
{
    public class CanvasWindows : ICanvas
    {
        #region Properties
            public bool Transform { set; get; }
        #endregion
        #region Constructors
            public CanvasWindows() 
            {
                this.Transform = true;
            }
        #endregion
        #region Create
            byte[] ICanvas.Create(int width, int height) 
            {
                Bitmap bitmap = new Bitmap(width, height);
                MemoryStream memo = new MemoryStream();
                bitmap.Save(memo, ImageFormat.Png);
                bitmap.Dispose();
                return (memo.ToArray());
            }
        #endregion
        #region Draw
            byte[] ICanvas.Draw(byte[] data, byte[] image, int x, int y, int width, int height)
            {
                //Source
                Bitmap bitmap = (Bitmap)Bitmap.FromStream(new MemoryStream(data.ToArray()));
                Graphics graphics = Graphics.FromImage(bitmap);
                //Image
                Bitmap imageDraw = (Bitmap)Bitmap.FromStream(new MemoryStream(image.ToArray()));
                graphics.DrawImage(imageDraw, x, y, width, height);
                MemoryStream memo = new MemoryStream();
                bitmap.Save(memo, ImageFormat.Png);
                bitmap.Dispose();
                return (memo.ToArray());
            }
        #endregion
        #region Extract
            byte[] ICanvas.Extract(Data.Image image, int x, int y, int width, int height, string transparency)
            {
                Bitmap bitmap = (Bitmap)Bitmap.FromStream(new MemoryStream(image.Data.ToArray()));
                Bitmap extracted = bitmap.Clone(new Rectangle(x, y, width, height), bitmap.PixelFormat);
                if (!string.IsNullOrEmpty(transparency))
                    extracted.MakeTransparent(this.GetColorEquivalent(transparency));
                MemoryStream memo = new MemoryStream();
                extracted.Save(memo, ImageFormat.Png);
                bitmap.Dispose();
                return (memo.ToArray());
            }

            private Color GetColorEquivalent(string transparency) 
            {
                return(Color.FromArgb(GetIntFromHexa(transparency.Substring(0,2)),GetIntFromHexa(transparency.Substring(2,2)), GetIntFromHexa(transparency.Substring(4,2))));
            }

            private int GetIntFromHexa(string hexa) 
            {
                return (Int32.Parse(hexa, NumberStyles.HexNumber));
            }
        #endregion
        #region Transform
            byte[] ICanvas.Transform(byte[] data) 
            {
                if (!this.Transform)
                    return (data);
                MemoryStream memo = new MemoryStream(data.ToArray());
                System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)Image.FromStream(memo);
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var size = new SharpDX.Size2(bitmap.Width, bitmap.Height);
                int width = bitmap.Width;
                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new SharpDX.DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }
                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;
                    List<byte> dataList = new List<byte>();
                    while(tempStream.RemainingLength > 0)
                        dataList.Add(tempStream.Read<byte>());
                    data = dataList.ToArray();
                }
                return (data);
            }
        #endregion
    }
}
