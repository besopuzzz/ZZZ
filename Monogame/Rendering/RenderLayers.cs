using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.Rendering
{
    public class RenderLayers
    {
        public List<RenderSheet> RenderSheets { get; } = new List<RenderSheet>();

        private Dictionary<RenderLayer, RenderComponents> layers = new Dictionary<RenderLayer, RenderComponents>();

        public void Initialize(GraphicsDevice graphicsDevice)
        {

            foreach (var item in layers.Values)
            {
                item.Initialize(graphicsDevice);
            }

        }

        public void Deinitialize()
        {

        }

        public void Install(RenderLayer layer, IRenderComponent component)
        {
            layers[layer].Add(component);
        }

        public void Uninstall(RenderLayer layer, IRenderComponent component)
        {
            layers[layer].Add(component);
        }


        public void Draw(RenderBatch spriteBatch, Matrix matrix)
        {
            foreach (var layer in layers)
            {
                RenderComponents renderComponents = layer.Value;


                renderComponents.Begin(SpriteSortMode.FrontToBack, null, null, null, null, null, null);
                renderComponents.Draw();
                renderComponents.End();

            }

            //spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var item in layers.Values)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate);
                spriteBatch.Draw(item.RenderTarget2D);
                spriteBatch.End();
            }

        }
    }
}
