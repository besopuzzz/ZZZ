using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.Rendering
{
    public class RenderController : MonogameController<IRenderComponent>
    {
        public Matrix? ViewMatrix => camera?.Projection;
        private RenderBatch renderBatch;
        private Dictionary<RenderLayer, RenderComponents> layers = new Dictionary<RenderLayer, RenderComponents>();
        private Camera camera;
        private Renderer renderer;
        private GraphicsDevice graphicsDevice;

        public RenderController()
        {
            string[] names = Enum.GetNames(typeof(RenderLayer));

            for (int i = 0; i < names.Length - 1; i++)
            {
                layers.Add((RenderLayer)Enum.Parse(typeof(RenderLayer), names[i]), new RenderComponents());
            }
        }

        protected override void Startup()
        {
            graphicsDevice = ((IGraphicsDeviceService)Game.Services.GetService(typeof(IGraphicsDeviceService))).GraphicsDevice;

            renderer = new Renderer(graphicsDevice);
            renderBatch = new RenderBatch(graphicsDevice);

            camera = FindComponent<Camera>();

            foreach (var item in layers.Values)
            {
                item.Initialize(graphicsDevice);
            }

            base.Startup();
        }

        protected override void Shutdown()
        {
            layers.Clear();

            base.Shutdown();
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                foreach (var item in layers.Values)
                {
                    item.Dispose();
                }

                ((IDisposable)camera)?.Dispose();
            }

            camera = null;
            layers = null;

            base.Dispose(disposing);
        }

        protected override void Reception(IRenderComponent component)
        {
            foreach (var layer in layers)
            {
                if (component.Layer.HasFlag(layer.Key))
                    layer.Value.Add(component);
            }

            component.LayerChanged += Component_LayerChanged;
        }

        protected override void Departure(IRenderComponent component)
        {
            //if (component == camera)
            //{
            //    camera = null;
            //    return;
            //}
            component.LayerChanged -= Component_LayerChanged;

            foreach (var layer in layers)
            {
                if (component.Layer.HasFlag(layer.Key))
                    layer.Value.Remove(component);
            }
        }

        private void Component_LayerChanged(object sender, RenderLayerEventArgs e)
        {
            foreach (var layer in layers)
            {
                if (e.OldValue.HasFlag(layer.Key))
                    layer.Value.Remove(e.Component);
            }

            foreach (var layer in layers)
            {
                if (e.Component.Layer.HasFlag(layer.Key))
                    layer.Value.Add(e.Component);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            renderer.SetGameTime(gameTime);

            Matrix? matrix = null;

            foreach (var layer in layers)
            {
                if(camera != null)
                {
                    if (!camera.Layer.HasFlag(layer.Key))
                        continue;

                    matrix = camera.Projection;
                }

                RenderComponents renderComponents = layer.Value;


                renderComponents.Begin(SpriteSortMode.Immediate, null, null, null, null, null, matrix);
                renderComponents.Draw();
                renderComponents.End();

            }

            graphicsDevice.Clear(Color.CornflowerBlue);

            renderBatch.Begin(SpriteSortMode.Immediate);

            foreach (var item in layers.Values)
            {
                renderBatch.Draw(item.RenderTarget2D);
            }

            renderBatch.End();
        }
    }

}
