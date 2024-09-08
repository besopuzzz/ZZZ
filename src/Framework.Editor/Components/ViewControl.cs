using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;
using System.Diagnostics;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Editor.Components
{
    public class ViewControl : Control, IGameInstance
    {
        [Browsable(false)]
        public GraphicsDevice GraphicsDevice => graphicsDeviceService?.GraphicsDevice;

        [Browsable(false)]
        public GameServiceContainer Services => services;

        [Browsable(false)]
        public GameComponentCollection Components => components;

        private GraphicsDeviceService graphicsDeviceService = null!;
        private GameServiceContainer services;
        private GameComponentCollection components;
        private GameTime gameTime;
        private Stopwatch timer;
        private TimeSpan elapsed;
        private CategorizedGameComponents gameComponents;

        public ViewControl()
        {
            services = new GameServiceContainer();
            components = new GameComponentCollection();
            gameComponents = new CategorizedGameComponents(components);
            gameTime = new GameTime();
        }

        protected override void OnCreateControl()
        {
            if (DesignMode)
                return;

            Mouse.WindowHandle = Handle;

            graphicsDeviceService = GraphicsDeviceService.AddRef(Handle, ClientSize.Width, ClientSize.Height, GraphicsProfile.Reach);

            services.AddService(typeof(IGraphicsDeviceService), graphicsDeviceService);

            Application.Idle -= GameLoop!;
            Application.Idle += GameLoop!;

            InitializeAll();

            base.OnCreateControl();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            if(timer != null)
            {
                if (Enabled)
                    timer.Start();
                else timer.Stop();
            }

            base.OnEnabledChanged(e);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            gameTime = new GameTime(timer.Elapsed, timer.Elapsed - elapsed);
            elapsed = timer.Elapsed;

            UpdateAll();

            Invalidate();
        }

        private void InitializeAll()
        {
            Initialize();

            gameComponents.Initialize();

            timer = Stopwatch.StartNew();
        }

        private void UpdateAll()
        {
            if (!Enabled)
                return;

            gameComponents.UpdateComponents(gameTime);

            Update(gameTime);
        }

        private void DrawAll()
        {
            if (!Enabled)
                return;

            Draw();

            gameComponents.DrawComponents(gameTime);

            GraphicsDevice.Present();
        }

        protected virtual void Initialize()
        {

        }

        protected virtual void Update(GameTime gameTime)
        {

        }

        protected virtual void Draw(GameTime gameTime)
        {

        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                timer?.Stop();
                graphicsDeviceService?.Release(disposing);
            }

            timer = null!;
            graphicsDeviceService = null!;

            base.Dispose(disposing);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (GraphicsDevice == null)
                e.Graphics.Clear(System.Drawing.Color.CornflowerBlue);
            else
                DrawAll();
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            graphicsDeviceService?.ResetDevice(ClientSize.Width, ClientSize.Height);

            base.OnClientSizeChanged(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected virtual void Draw()
        {
            GraphicsDevice?.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
        }
    }
}