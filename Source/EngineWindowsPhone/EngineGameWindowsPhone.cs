using System;
using System.Diagnostics;
using SharpDX;
using SharpDX.Toolkit;
using System.Reflection;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace SpriteBatchAndFont
{
    using SharpDX.Toolkit.Graphics;
    using Engine;
    using EngineWindowsPhone;
    using System.Collections.Generic;
    using Windows.UI.Xaml;

    public class EngineGameWindowsPhone : Game
    {
        private Windows.UI.Xaml.Controls.Page _page;
        public GameEngine _game = null;
        private GraphicsDeviceManager graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private BitmapProperties _bitmapProperties = new BitmapProperties(new SharpDX.Direct2D1.PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
        private BitmapManager<Texture2D> _cacheBitmaps = new BitmapManager<Texture2D>();

        public int ScreenWidth{set;get;}
        public int ScreenHeight { set; get; }

        public EngineGameWindowsPhone(Windows.UI.Xaml.Controls.Page page)
        {
            this._page = page;
            graphicsDeviceManager = new GraphicsDeviceManager(this); 
            graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            Content.RootDirectory = "Content";
            QueryManager query = new QueryManager(new QueryWindowsPhone());
            NetworkManager network = new NetworkManager(new NetworkWindowsPhone(), new PackageManager(), query.Get(QueryType.MasterHost), Int32.Parse(query.Get(QueryType.MasterPort)));
            this._game = new GameEngine(new ResourceManager(new StorageWindowsPhone(page.GetType().GetTypeInfo().Assembly)), new Canvas(new CanvasWindowsPhone()), network, query);
            this._game.Keyboard = (new Windows.Devices.Input.TouchCapabilities()).TouchPresent > 0;
        }

        protected override void Initialize()
        {
            this._game.Initialize("Airplane");
            Window.Title = "Red Baron 1918";
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            _spriteBatch.Dispose();
            base.UnloadContent();
        }

        public Texture2D CreateBitmap(Data.Layer layer)
        {
            if ((!layer.Code.HasValue) || (!this._cacheBitmaps.Contains(layer.Code.Value)))
            {
                Image image = Image.New2D(layer.Width, layer.Height, 1, PixelFormat.R8G8B8A8.UNorm);
                image.PixelBuffer[0].SetPixels<byte>(layer.Data);
                Texture2D bitmap = Texture2D.New(this.GraphicsDevice, image);
                if (!layer.Code.HasValue)
                    return (bitmap);
                this._cacheBitmaps.Add(layer.Code.Value, bitmap);
            }
            return (this._cacheBitmaps.GetBitmap(layer.Code.Value));
        }

        private Texture2D CreateRectangle(Data.Box box)
        {
            int width = 1;
            int height = 1;
            Image image = Image.New2D(width, height, 1, PixelFormat.R8G8B8A8.UNorm);
            List<byte> data = new List<byte>();
            List<byte> pixelData = new List<byte>();
            pixelData.Add((byte)box.Color.Red);
            pixelData.Add((byte)box.Color.Green);
            pixelData.Add((byte)box.Color.Blue);
            pixelData.Add((byte)box.Color.Alpha);
            int pixels = width * height;
            for (int i = 0; i < pixels; i++)
                data.AddRange(pixelData);
            image.PixelBuffer[0].SetPixels<byte>(data.ToArray());
            Texture2D bitmap = Texture2D.New(this.GraphicsDevice, image);
            image.Dispose();
            return (bitmap);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (this._game.ViewWidth != this.ScreenWidth)
            {
                //Initialize
                this._game.ViewWidth = this.ScreenWidth;
                this._game.ViewHeight = this.ScreenHeight;
            }
            if (this._game.Exit)
                Application.Current.Exit();
            //Update
            this._game.Update();
            // Draw 
            List<Data.Layer> layers = this._game.Draw();
            if (layers == null)
                return;
            this._cacheBitmaps.Start();
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.Clear(Color.Transparent);
            List<int> layersCodes = new List<int>();
            for (int i = 0; i < layers.Count; i++)
            {
                //Layer
                Data.Layer layer = layers[i];
                if (layer.Data != null)
                {
                    Texture2D bitmap = CreateBitmap(layer);
                    if (!layer.Code.HasValue)
                    {
                        this._spriteBatch.Begin();
                        this._spriteBatch.Draw(bitmap, new SharpDX.RectangleF(layer.X, layer.Y, layer.Width * this._game.Scale, layer.Height * this._game.Scale), Color.White);
                        this._spriteBatch.End();
                    }else if (!layersCodes.Contains(layer.Code.Value)){
                        List<Data.Layer> layersDraw = layers.FindAll(l => l.Code == layer.Code);
                        foreach (Data.Layer layerDraw in layersDraw)
                        {
                            this._spriteBatch.Begin();
                            this._spriteBatch.Draw(bitmap, new SharpDX.RectangleF(layerDraw.X, layerDraw.Y, layerDraw.Width * this._game.Scale, layerDraw.Height * this._game.Scale), Color.White);
                            this._spriteBatch.End();
                        }
                        layersCodes.Add(layer.Code.Value);
                    }
                }
                //Boxes
                foreach (Data.Box box in layer.Boxes)
                {
                    Texture2D bitmap = CreateRectangle(box);
                    this._spriteBatch.Begin();
                    this._spriteBatch.Draw(bitmap, new SharpDX.RectangleF(box.Rectangle.X, box.Rectangle.Y, box.Rectangle.Width, box.Rectangle.Height), Color.White);
                    this._spriteBatch.End();
                    bitmap.Dispose();
                }
            }
            this._cacheBitmaps.End();
            base.Draw(gameTime);
        }

        public void TouchDown(int x, int y, int code)
        {
            this._game.Touch(x, y, true, code);
        }

        public void TouchUp(int x, int y, int code)
        {
            this._game.Touch(x, y, false, code);
        }
    }
}
