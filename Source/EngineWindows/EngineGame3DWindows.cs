using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D;
using SharpDX.Direct3D10;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D10.Device;
using Device1 = SharpDX.Direct3D10.Device1;
using DriverType = SharpDX.Direct3D10.DriverType;
using FeatureLevel = SharpDX.Direct3D10.FeatureLevel;
using SharpDX;

namespace EngineWindows
{
    public class EngineGame3DWindows : EngineGameWindows
    {
        Device1 _device;
        SwapChain _swapChain;
        Texture2D _backBuffer;
        RenderTargetView _backBufferView;

        /// <summary>
        /// Returns the device
        /// </summary>
        public Device1 Device
        {
            get
            {
                return _device;
            }
        }

        /// <summary>
        /// Returns the backbuffer used by the SwapChain
        /// </summary>
        public Texture2D BackBuffer
        {
            get
            {
                return _backBuffer;
            }
        }

        /// <summary>
        /// Returns the render target view on the backbuffer used by the SwapChain.
        /// </summary>
        public RenderTargetView BackBufferView
        {
            get
            {
                return _backBufferView;
            }
        }

        protected override void Initialize(EngineConfiguration demoConfiguration)
        {
            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(demoConfiguration.Width, demoConfiguration.Height,
                                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = DisplayHandle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            Device1.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, desc, FeatureLevel.Level_10_0, out _device, out _swapChain);

            // Ignore all windows events
            Factory factory = _swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(DisplayHandle, WindowAssociationFlags.IgnoreAll);

            // New RenderTargetView from the backbuffer
            _backBuffer = Texture2D.FromSwapChain<Texture2D>(_swapChain, 0);

            _backBufferView = new RenderTargetView(_device, _backBuffer);


        }

        protected override void BeginDraw()
        {
            base.BeginDraw();
            Device.Rasterizer.SetViewports(new Viewport(0, 0, Config.Width, Config.Height));
            Device.OutputMerger.SetTargets(_backBufferView);
        }


        protected override void EndDraw()
        {
            _swapChain.Present(Config.WaitVerticalBlanking ? 1 : 0, PresentFlags.None);
        }
    }
}
