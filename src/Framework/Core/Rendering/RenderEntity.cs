using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering
{
    internal class RenderEntity
    {
        public int Depth { get; }
        public List<IRender> Components { get; }

        public RenderEntity Next { get; }

        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        public  RenderEntity(GraphicsDevice device)
        {
            graphicsDevice = device;
            spriteBatch = new SpriteBatch(graphicsDevice);

            Depth = 0;
            Components = new List<IRender>();

            Next = new RenderEntity(Depth++);
        }

        private RenderEntity(int depth)
        {
            Depth = depth;

            Components = new List<IRender>();
            Next = new RenderEntity(Depth++);
        }

        public void Start()
        {
            spriteBatch.Begin();
        }

        public void Render()
        {
            foreach (var item in Components)
            {
                // ... 
            }
        }

        public void End()
        {
            spriteBatch.End();
        }
    }
}
