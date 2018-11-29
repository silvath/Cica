using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using Bitmap = SharpDX.Direct2D1.Bitmap;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Engine;
using EngineWindows;
using System.IO;
using System.Drawing;
using Data;
using System.Configuration;

namespace AirplaneWindows
{
    public class Program : EngineGame2DWindows
    {
        #region Attributes
            public GameEngine _game = null;
        #endregion

        [STAThread]
        static void Main()
        {
            Program program = new Program();
            program.Run(new EngineConfiguration("Airplane",1000,600));
        }

        protected override void Initialize(EngineConfiguration demoConfiguration)
        {
            QueryManager query = new QueryManager(new QueryWindows());
            NetworkManager network = new NetworkManager(new NetworkWindows(), new PackageManager(), query.Get(QueryType.MasterHost), Int32.Parse(query.Get(QueryType.MasterPort)));
            this._game = new GameEngine(new ResourceManager(new StorageWindows(StorageWindows.GetPathFolderResources())), new Canvas(new CanvasWindows()), network, query);
            base.Initialize(demoConfiguration);
            this._game.Debug = false;
            this._game.Initialize("Airplane");
            this._game.ViewWidth = demoConfiguration.Width;
            this._game.ViewHeight = demoConfiguration.Height;
        }

        protected override void Draw(EngineTime time)
        {
            base.Draw(time);
            if (this._game.Exit)
                this.Exit();
            //Update
            this._game.Update();
            // Draw 
            List<Data.Layer> layers = this._game.Draw();
            if (layers == null)
                return;
            RenderTarget2D.FillRectangle(new SharpDX.RectangleF(0, 0, this._game.ViewWidth, this._game.ViewHeight), new SolidColorBrush(RenderTarget2D, new Color4(new Color3(0, 0, 0))));
            List<int> layersCodes = new List<int>();
            for(int i = 0; i < layers.Count;i++)
            {
                //Layer
                Data.Layer layer = layers[i];
                if (layer.Data != null)
                {
                    Bitmap bitmap = CreateBitmap(RenderTarget2D, layer);
                    if (!layer.Code.HasValue)
                    {
                        RenderTarget2D.DrawBitmap(bitmap, new SharpDX.RectangleF(layer.X, layer.Y, layer.Width, layer.Height), 1.0f, BitmapInterpolationMode.Linear);
                    }else if (!layersCodes.Contains(layer.Code.Value)){
                        List<Data.Layer> layersDraw = layers.FindAll(l => l.Code == layer.Code);
                        foreach(Data.Layer layerDraw in layersDraw)
                            RenderTarget2D.DrawBitmap(bitmap, new SharpDX.RectangleF(layerDraw.X, layerDraw.Y, layerDraw.Width, layerDraw.Height), 1.0f, BitmapInterpolationMode.Linear);
                        layersCodes.Add(layer.Code.Value);
                    }
                }
                //Boxes
                foreach (Data.Box box in layer.Boxes)
                    RenderTarget2D.FillRectangle(new SharpDX.RectangleF(box.Rectangle.X, box.Rectangle.Y, box.Rectangle.Width, box.Rectangle.Height), new SolidColorBrush(RenderTarget2D, new Color4(new Color3(box.Color.Red, box.Color.Green, box.Color.Blue), box.Color.Alpha)));
            }
            //Inputs
            if (this._game.Debug) 
            {
                SolidColorBrush brush = new SolidColorBrush(RenderTarget2D, new Color4(new Color3(255, 0, 0), 0.5f));
                int x = 30;
                int y = 30;
                List<List<InputType>> buffer = this._game.InputBuffer();
                for (int i = buffer.Count - 1; i >= 0; i--) 
                {
                    List<InputType> inputs = buffer[i];
                    string text = (inputs.Count == 0) ? "-" : GetInputText(inputs[0]);
                    RenderTarget2D.DrawText(text, new SharpDX.DirectWrite.TextFormat(new SharpDX.DirectWrite.Factory(), "Arial", 22), new SharpDX.RectangleF(x, y, 20, 20), brush);
                    x += 20;
                }
            }
        }

        private string GetInputText(InputType input) 
        {
            if (input == InputType.Left)
                return ("<");
            else if (input == InputType.Right)
                return (">");
            else if (input == InputType.Button1)
                return ("1");
            return ("-");
        }

        protected override void KeyDown(KeyEventArgs e)
        {
            base.KeyDown(e);
            InputType? input = this.GetInput(e);
            if (input.HasValue)
                this._game.Input(input.Value, true);
        }

        protected override void KeyUp(KeyEventArgs e)
        {
            base.KeyUp(e);
            InputType? input = this.GetInput(e);
            if (input.HasValue)
                this._game.Input(input.Value, false);
        }

        private InputType? GetInput(KeyEventArgs e) 
        {
            if (e.KeyCode == Keys.Left)
                return (InputType.Left);
            else if (e.KeyCode == Keys.Right)
                return (InputType.Right);
            else if (e.KeyCode == Keys.Up)
                return (InputType.Button1);
            else if (e.KeyCode == Keys.Space)
                return (InputType.Right);
            else if (e.KeyCode == Keys.Down)
                return (InputType.Down);
            else if (e.KeyCode == Keys.Delete)
                return (InputType.Pause);
            else if (e.KeyCode == Keys.PageDown)
                return (InputType.Frame);
            else if (e.KeyCode == Keys.Escape)
                return (InputType.Escape);
            return (null);
        }

        protected override void MouseDown(MouseEventArgs e)
        {
            base.MouseDown(e);
            this._game.Touch(e.X, e.Y, true,null);
        }

        protected override void MouseUp(MouseEventArgs e)
        {
            base.MouseUp(e);
            this._game.Touch(e.X, e.Y, false,null);
        }
    }
}
