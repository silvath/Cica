using CommonDX;
using Engine;
using EngineWindowsStore;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.UI.Xaml.Input;

namespace AirplaneWindowsStore
{
    public class EngineRenderer : Component
    {
        #region Attributes
            public GameEngine _game = null;
            private DeviceManager _deviceManager;
            private BitmapProperties _bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            private BitmapManager<Bitmap> _cacheBitmaps = new BitmapManager<Bitmap>();
        #endregion 
        #region Properties
            private SwapChainBackgroundPanelTarget Target { set; get; }
            private int ScreenWidth
            {
                get 
                {
                    return (this.Target.ScreenWidth);
                }
            }
            private int ScreenHeight
            {
                get
                {
                    return (this.Target.ScreenHeight);
                }
            }
        #endregion
        #region Constructors
            public EngineRenderer(SwapChainBackgroundPanelTarget target) 
            {
                this.Target = target;
            } 
        #endregion
        #region Initialize
            public virtual void Initialize(DeviceManager deviceManager)
            {
                this._deviceManager = deviceManager;
                QueryManager query = new QueryManager(new QueryWindowsStore());
                NetworkManager network = new NetworkManager(new NetworkWindowsStore(), new PackageManager(), query.Get(QueryType.MasterHost), Int32.Parse(query.Get(QueryType.MasterPort)));
                this._game = new GameEngine(new ResourceManager(new StorageWindowsStore(this.GetType().GetTypeInfo().Assembly)), new Canvas(new CanvasWindowsStore()), network, query);
                this._game.Keyboard = (new Windows.Devices.Input.TouchCapabilities()).TouchPresent > 0;
                this._game.Initialize("Airplane");
            }
        #endregion

       #region Render
            public virtual void Render(TargetBase target)
            {
                var context2D = target.DeviceManager.ContextDirect2D;
                if (this._game.ViewWidth != this.ScreenWidth)
                {
                    //Initialize
                    this._game.ViewWidth = this.ScreenWidth;
                    this._game.ViewHeight = this.ScreenHeight;
                }
                context2D.BeginDraw();
                this._cacheBitmaps.Start();
                if (this._game.Exit)
                    App.Current.Exit();
                //Update
                this._game.Update();
                // Draw 
                List<Data.Layer> layers = this._game.Draw();
                if (layers == null)
                    return;
                context2D.Clear(Color.Black);
                context2D.Clear(Color.Transparent);
                List<int> layersCodes = new List<int>();
                for (int i = 0; i < layers.Count; i++)
                {
                    //Layer
                    Data.Layer layer = layers[i];
                    if (layer.Data != null)
                    {
                        Bitmap bitmap = CreateBitmap(context2D, layer);
                        if (!layer.Code.HasValue)
                        {
                            context2D.DrawBitmap(bitmap, new SharpDX.RectangleF(layer.X, layer.Y, layer.Width * this._game.Scale, layer.Height * this._game.Scale), 1.0f, BitmapInterpolationMode.Linear);
                        }else if (!layersCodes.Contains(layer.Code.Value)){
                            List<Data.Layer> layersDraw = layers.FindAll(l => l.Code == layer.Code);
                            foreach (Data.Layer layerDraw in layersDraw)
                                context2D.DrawBitmap(bitmap, new SharpDX.RectangleF(layerDraw.X, layerDraw.Y, layerDraw.Width * this._game.Scale, layer.Height * this._game.Scale), 1.0f, BitmapInterpolationMode.Linear);
                            layersCodes.Add(layer.Code.Value);
                        }
                    }
                    //Boxes
                    foreach (Data.Box box in layer.Boxes)
                        context2D.FillRectangle(new SharpDX.RectangleF(box.Rectangle.X, box.Rectangle.Y, box.Rectangle.Width, box.Rectangle.Height), new SolidColorBrush(context2D, new Color4(new Color3(box.Color.Red, box.Color.Green, box.Color.Blue), box.Color.Alpha)));
                }
                //Inputs
                if (this._game.Debug)
                {
                    SolidColorBrush brush = new SolidColorBrush(context2D, new Color4(new Color3(255, 0, 0), 0.5f));
                    int x = 30;
                    int y = 30;
                    List<List<Data.InputType>> buffer = this._game.InputBuffer();
                    for (int i = buffer.Count - 1; i >= 0; i--)
                    {
                        List<Data.InputType> inputs = buffer[i];
                        string text = (inputs.Count == 0) ? "-" : GetInputText(inputs[0]);
                        context2D.DrawText(text, new SharpDX.DirectWrite.TextFormat(new SharpDX.DirectWrite.Factory(), "Arial", 22), new SharpDX.RectangleF(x, y, 20, 20), brush);
                        x += 20;
                    }
                }
                this._cacheBitmaps.End();
                context2D.EndDraw();
            }

            private string GetInputText(Data.InputType input)
            {
                if (input == Data.InputType.Left)
                    return ("<");
                else if (input == Data.InputType.Right)
                    return (">");
                else if (input == Data.InputType.Button1)
                    return ("1");
                return ("-");
            }
       #endregion
       #region Input
            public void KeyDown(KeyRoutedEventArgs e)
            {
                Data.InputType? input = this.GetInput(e);
                if (input.HasValue)
                    this._game.Input(input.Value, true);
            }

            public void KeyUp(KeyRoutedEventArgs e)
            {
                Data.InputType? input = this.GetInput(e);
                if (input.HasValue)
                    this._game.Input(input.Value, false);
            }

            private Data.InputType? GetInput(KeyRoutedEventArgs e)
            {
                if (e.Key == Windows.System.VirtualKey.Left)
                    return (Data.InputType.Left);
                else if (e.Key == Windows.System.VirtualKey.Right)
                    return (Data.InputType.Right);
                else if (e.Key == Windows.System.VirtualKey.Up)
                    return (Data.InputType.Button1);
                else if (e.Key == Windows.System.VirtualKey.Space)
                    return (Data.InputType.Right);
                else if (e.Key == Windows.System.VirtualKey.Down)
                    return (Data.InputType.Down);
                else if (e.Key == Windows.System.VirtualKey.Delete)
                    return (Data.InputType.Pause);
                else if (e.Key == Windows.System.VirtualKey.PageDown)
                    return (Data.InputType.Frame);
                else if (e.Key == Windows.System.VirtualKey.Escape)
                    return (Data.InputType.Escape);
                return (null);
            }

            public void TouchDown(int x, int y, int code) 
            {
                this._game.Touch(x, y, true, code);
            }

            public void TouchUp(int x, int y, int code)
            {
                this._game.Touch(x, y, false, code);
            }
       #endregion

        #region Bitmap
            protected Bitmap CreateBitmap(RenderTarget renderTarget, Data.Layer layer)
            {
                if ((!layer.Code.HasValue) || (!this._cacheBitmaps.Contains(layer.Code.Value)))
                {
                    Size2 size = new Size2(layer.Width, layer.Height);
                    int stride = layer.Width * sizeof(int);
                    Bitmap bitmap = null;
                    using (DataStream tempStream = new DataStream(layer.Height * stride, true, true))
                    {
                        tempStream.WriteRange<byte>(layer.Data.ToArray());
                        tempStream.Position = 0;
                        bitmap = new Bitmap(renderTarget, size, tempStream, stride, this._bitmapProperties);
                    }
                    if (!layer.Code.HasValue)
                        return (bitmap);
                    this._cacheBitmaps.Add(layer.Code.Value, bitmap);
                }
                return (this._cacheBitmaps.GetBitmap(layer.Code.Value));
            }
        #endregion
    }
}
