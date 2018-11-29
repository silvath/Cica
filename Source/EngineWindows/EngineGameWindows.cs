using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineWindows
{
    public abstract class EngineGameWindows
    {
        #region Attributes
            private readonly EngineTime clock = new EngineTime();
            private FormWindowState _currentFormWindowState;
            private bool _disposed;
            private Form _form;
            private float _frameAccumulator;
            private int _frameCount;
            private EngineConfiguration _demoConfiguration;
        #endregion
        #region Properties
            protected IntPtr DisplayHandle
            {
                get
                {
                    return _form.Handle;
                }
            }
            public EngineConfiguration Config
            {
                get
                {
                    return _demoConfiguration;
                }
            }
            public float FrameDelta { get; private set; }
            public float FramePerSecond { get; private set; }
        #endregion

        #region Constructors
            public EngineGameWindows() 
            {

            }

            ~EngineGameWindows() 
            {
                if (!_disposed)
                {
                    Dispose(false);
                    _disposed = true;
                }
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    Dispose(true);
                    _disposed = true;
                }
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposeManagedResources)
            {
                if (disposeManagedResources)
                {
                    if (_form != null)
                        _form.Dispose();
                }
            }
        #endregion

        #region Others
            protected virtual Form CreateForm(EngineConfiguration config)
            {
                return new RenderForm(config.Title)
                {
                    ClientSize = new System.Drawing.Size(config.Width, config.Height),
                    StartPosition = FormStartPosition.CenterScreen
                };
            }

            /// <summary>
            /// Runs the demo with default presentation
            /// </summary>
            public void Run()
            {
                Run(new EngineConfiguration());
            }

            /// <summary>
            /// Runs the demo.
            /// </summary>
            public void Run(EngineConfiguration demoConfiguration)
            {
                _demoConfiguration = demoConfiguration ?? new EngineConfiguration();
                _form = CreateForm(_demoConfiguration);
                Initialize(_demoConfiguration);

                bool isFormClosed = false;
                bool formIsResizing = false;

                _form.MouseClick += HandleMouseClick;
                _form.MouseDown += HandleMouseDown;
                _form.MouseUp += HandleMouseUp;
                _form.KeyDown += HandleKeyDown;
                _form.KeyUp += HandleKeyUp;
                _form.Resize += (o, args) =>
                {
                    if (_form.WindowState != _currentFormWindowState)
                    {
                        HandleResize(o, args);
                    }

                    _currentFormWindowState = _form.WindowState;
                };

                _form.ResizeBegin += (o, args) => { formIsResizing = true; };
                _form.ResizeEnd += (o, args) =>
                {
                    formIsResizing = false;
                    HandleResize(o, args);
                };

                _form.Closed += (o, args) => { isFormClosed = true; };

                LoadContent();

                clock.Start();
                BeginRun();
                RenderLoop.Run(_form, () =>
                {
                    if (isFormClosed)
                    {
                        return;
                    }

                    OnUpdate();
                    if (!formIsResizing)
                        Render();
                });

                UnloadContent();
                EndRun();

                // Dispose explicity
                Dispose();
            }

            /// <summary>
            ///   In a derived class, implements logic to initialize the sample.
            /// </summary>
            protected abstract void Initialize(EngineConfiguration demoConfiguration);

            protected virtual void LoadContent()
            {
            }

            protected virtual void UnloadContent()
            {
            }

            /// <summary>
            ///   In a derived class, implements logic to update any relevant sample state.
            /// </summary>
            protected virtual void Update(EngineTime time)
            {
            }

            /// <summary>
            ///   In a derived class, implements logic to render the sample.
            /// </summary>
            protected virtual void Draw(EngineTime time)
            {
            }

            protected virtual void BeginRun()
            {
            }

            protected virtual void EndRun()
            {
            }

            /// <summary>
            ///   In a derived class, implements logic that should occur before all
            ///   other rendering.
            /// </summary>
            protected virtual void BeginDraw()
            {
            }

            /// <summary>
            ///   In a derived class, implements logic that should occur after all
            ///   other rendering.
            /// </summary>
            protected virtual void EndDraw()
            {
            }

            /// <summary>
            ///   Quits the sample.
            /// </summary>
            public void Exit()
            {
                _form.Close();
            }

            /// <summary>
            ///   Updates sample state.
            /// </summary>
            private void OnUpdate()
            {
                FrameDelta = (float)clock.Update();
                Update(clock);
            }

            protected System.Drawing.Size RenderingSize
            {
                get
                {
                    return _form.ClientSize;
                }
            }

            /// <summary>
            ///   Renders the sample.
            /// </summary>
            private void Render()
            {
                _frameAccumulator += FrameDelta;
                ++_frameCount;
                if (_frameAccumulator >= 1.0f)
                {
                    FramePerSecond = _frameCount / _frameAccumulator;

                    _form.Text = _demoConfiguration.Title + " - FPS: " + FramePerSecond;
                    _frameAccumulator = 0.0f;
                    _frameCount = 0;
                }

                BeginDraw();
                Draw(clock);
                EndDraw();
            }

            protected virtual void MouseClick(MouseEventArgs e)
            {
            }

            protected virtual void MouseDown(MouseEventArgs e)
            {
            }

            protected virtual void MouseUp(MouseEventArgs e)
            {
            }

            protected virtual void KeyDown(KeyEventArgs e)
            {
            }

            protected virtual void KeyUp(KeyEventArgs e)
            {
            }

            /// <summary>
            ///   Handles a mouse click event.
            /// </summary>
            /// <param name = "sender">The sender.</param>
            /// <param name = "e">The <see cref = "System.Windows.Forms.MouseEventArgs" /> instance containing the event data.</param>
            private void HandleMouseClick(object sender, MouseEventArgs e)
            {
                MouseClick(e);
            }

            private void HandleMouseDown(object sender, MouseEventArgs e)
            {
                MouseDown(e);
            }

            private void HandleMouseUp(object sender, MouseEventArgs e)
            {
                MouseUp(e);
            }

            /// <summary>
            ///   Handles a key down event.
            /// </summary>
            /// <param name = "sender">The sender.</param>
            /// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
            private void HandleKeyDown(object sender, KeyEventArgs e)
            {
                KeyDown(e);
            }

            /// <summary>
            ///   Handles a key up event.
            /// </summary>
            /// <param name = "sender">The sender.</param>
            /// <param name = "e">The <see cref = "System.Windows.Forms.KeyEventArgs" /> instance containing the event data.</param>
            private void HandleKeyUp(object sender, KeyEventArgs e)
            {
                KeyUp(e);
            }

            private void HandleResize(object sender, EventArgs e)
            {
                if (_form.WindowState == FormWindowState.Minimized)
                {
                    return;
                }

                // UnloadContent();

                //_configuration.WindowWidth = _form.ClientSize.Width;
                //_configuration.WindowHeight = _form.ClientSize.Height;

                //if( Context9 != null ) {
                //    userInterfaceRenderer.Dispose();

                //    Context9.PresentParameters.BackBufferWidth = _configuration.WindowWidth;
                //    Context9.PresentParameters.BackBufferHeight = _configuration.WindowHeight;

                //    Context9.Device.Reset( Context9.PresentParameters );

                //    userInterfaceRenderer = new UserInterfaceRenderer9( Context9.Device, _form.ClientSize.Width, _form.ClientSize.Height );
                //} else if( Context10 != null ) {
                //    userInterfaceRenderer.Dispose();

                //    Context10.SwapChain.ResizeBuffers( 1, WindowWidth, WindowHeight, Context10.SwapChain.Description.ModeDescription.Format, Context10.SwapChain.Description.Flags );


                //    userInterfaceRenderer = new UserInterfaceRenderer10( Context10.Device, _form.ClientSize.Width, _form.ClientSize.Height );
                //}

                // LoadContent();
            }
        #endregion
    }
}
