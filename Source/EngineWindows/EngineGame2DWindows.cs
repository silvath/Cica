using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Factory = SharpDX.Direct2D1.Factory;

namespace EngineWindows
{
    public class EngineGame2DWindows : EngineGame3DWindows
    {
        public Factory Factory2D { get; private set; }
        public SharpDX.DirectWrite.Factory FactoryDWrite { get; private set; }
        public RenderTarget RenderTarget2D { get; private set; }
        public SolidColorBrush SceneColorBrush { get; private set; }
        private BitmapProperties _bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, AlphaMode.Premultiplied));
        private Engine.BitmapManager<Bitmap> _cacheBitmaps = new Engine.BitmapManager<Bitmap>();

        protected override void Initialize(EngineConfiguration demoConfiguration)
        {
            base.Initialize(demoConfiguration);
            Factory2D = new SharpDX.Direct2D1.Factory();
            using (var surface = BackBuffer.QueryInterface<Surface>())
            {
                RenderTarget2D = new RenderTarget(Factory2D, surface,
                                                  new RenderTargetProperties(new PixelFormat(Format.Unknown, AlphaMode.Premultiplied)));
            }
            RenderTarget2D.AntialiasMode = AntialiasMode.PerPrimitive;

            FactoryDWrite = new SharpDX.DirectWrite.Factory();

            SceneColorBrush = new SolidColorBrush(RenderTarget2D, Color.White);
        }

        protected override void BeginDraw()
        {
            base.BeginDraw();
            RenderTarget2D.BeginDraw();
            this._cacheBitmaps.Start();
        }

        protected override void EndDraw()
        {
            this._cacheBitmaps.End();
            RenderTarget2D.EndDraw();
            base.EndDraw();
        }

        protected Bitmap CreateBitmap(RenderTarget renderTarget, Data.Layer layer)
        {
            if ((!layer.Code.HasValue) || (!this._cacheBitmaps.Contains(layer.Code.Value)))
            {
                Size2 size = new Size2(layer.Width, layer.Height);
                int stride = layer.Width * sizeof(int);
                DataStream tempStream = new DataStream(layer.Height * stride, true, true);
                tempStream.WriteRange<byte>(layer.Data);
                tempStream.Position = 0;
                Bitmap bitmap = new Bitmap(renderTarget, size, tempStream, stride, this._bitmapProperties);
                tempStream.Close();
                if(!layer.Code.HasValue)
                    return (bitmap);
                this._cacheBitmaps.Add(layer.Code.Value, bitmap);
            }
            return(this._cacheBitmaps.GetBitmap(layer.Code.Value));
        }
    }
}
