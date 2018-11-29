using SpriteBatchAndFont;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace AirplaneWindowsPhone
{
    public sealed partial class MainPage : Page
    {
        #region Attributes
            private EngineGameWindowsPhone _engine;
        #endregion
        #region Properties
            double ScaleX { set; get; }
            double ScaleY { set; get; }
        #endregion

        public MainPage()
        {
            SetScale(Windows.Graphics.Display.DisplayInformation.GetForCurrentView().ResolutionScale);
            InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            Windows.Graphics.Display.DisplayInformation.GetForCurrentView().OrientationChanged += MainPage_OrientationChanged;
            Windows.UI.Core.CoreWindow window = Windows.UI.Core.CoreWindow.GetForCurrentThread();
            _engine = new EngineGameWindowsPhone(this);
            _engine.Run(DrawingSurface);
            SetResolution();
        }

        void MainPage_OrientationChanged(Windows.Graphics.Display.DisplayInformation sender, object args)
        {
            SetResolution();
        }

        private void SetScale(Windows.Graphics.Display.ResolutionScale resolution)
        {
            switch (resolution)
            {
                case Windows.Graphics.Display.ResolutionScale.Scale100Percent:
                    this.ScaleX = this.ScaleY = 1;
                    break;
                case Windows.Graphics.Display.ResolutionScale.Scale120Percent:
                    this.ScaleX = this.ScaleY = 1.2;
                    break;
                case Windows.Graphics.Display.ResolutionScale.Scale140Percent:
                    this.ScaleX = this.ScaleY = 1.4;
                    break;
                case Windows.Graphics.Display.ResolutionScale.Scale150Percent:
                    this.ScaleX = this.ScaleY = 1.5;
                    break;
                case Windows.Graphics.Display.ResolutionScale.Scale160Percent:
                    this.ScaleX = this.ScaleY = 1.6;
                    break;
                case Windows.Graphics.Display.ResolutionScale.Scale180Percent:
                    this.ScaleX = 1.8;
                    this.ScaleY = 2;
                    break;
                case Windows.Graphics.Display.ResolutionScale.Scale225Percent:
                    this.ScaleX = this.ScaleY = 2.25;
                    break;
            }
        }

        private void SetResolution()
        {
            Windows.UI.Core.CoreWindow window = Windows.UI.Core.CoreWindow.GetForCurrentThread();
            _engine.ScreenWidth = (int)(window.Bounds.Width * this.ScaleX);
            _engine.ScreenHeight = (int)(window.Bounds.Height * this.ScaleY);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetResolution();
        }

        private void Page_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point point = e.GetCurrentPoint(this).Position;
            int x = (int)(point.X * this.ScaleX) - 50;
            int y = (int)(point.Y * this.ScaleY) - 32;
            _engine.TouchDown(x, y, (int)e.Pointer.PointerId);
        }

        private void Page_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Point point = e.GetCurrentPoint(null).Position;
            int x = (int)(point.X * this.ScaleX) - 50;
            int y = (int)(point.Y * this.ScaleY) - 32;
            _engine.TouchUp(x, y, (int)e.Pointer.PointerId);
        }
    }
}
