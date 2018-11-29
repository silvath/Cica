using CommonDX;
using EngineWindowsStore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AirplaneWindowsStore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Screen : SwapChainBackgroundPanel
    {
        #region Attributes
            private DeviceManager deviceManager;
            private SwapChainBackgroundPanelTarget d2dTarget;
            private EngineRenderer engineRenderer;
        #endregion
        #region Constructors
            public Screen()
            {
                this.InitializeComponent();
                //Hide Mouse Cursor
                Window.Current.CoreWindow.PointerCursor = null;
                d2dTarget = new SwapChainBackgroundPanelTarget(rootSCBP);
                engineRenderer = new EngineRenderer(d2dTarget);
                d2dTarget.OnRender += engineRenderer.Render;
                deviceManager = new DeviceManager();
                deviceManager.OnInitialize += d2dTarget.Initialize;
                deviceManager.OnInitialize += engineRenderer.Initialize;

                #if DEBUG
                var fpsRenderer = new FpsRenderer();
                d2dTarget.OnRender += fpsRenderer.Render;
                deviceManager.OnInitialize += fpsRenderer.Initialize;
                #endif
                deviceManager.Initialize(DisplayProperties.LogicalDpi);

                // Setup rendering callback
                CompositionTarget.Rendering += CompositionTarget_Rendering;
            }

            void CompositionTarget_Rendering(object sender, object e)
            {
                d2dTarget.RenderAll();
                d2dTarget.Present();
            }
        #endregion

        #region Events
            private void rootSCBP_KeyDown(object sender, KeyRoutedEventArgs e)
            {
                engineRenderer.KeyDown(e);
            }

            private void rootSCBP_KeyUp(object sender, KeyRoutedEventArgs e)
            {
                engineRenderer.KeyUp(e);
            }

            private void rootSCBP_PointerPressed(object sender, PointerRoutedEventArgs e)
            {
                engineRenderer.TouchDown((int)e.GetCurrentPoint(null).Position.X, (int)e.GetCurrentPoint(null).Position.Y, (int)e.Pointer.PointerId);
            }

            private void rootSCBP_PointerReleased(object sender, PointerRoutedEventArgs e)
            {
                engineRenderer.TouchUp((int)e.GetCurrentPoint(null).Position.X, (int)e.GetCurrentPoint(null).Position.Y, (int)e.Pointer.PointerId);
            }
        #endregion
    }
}
