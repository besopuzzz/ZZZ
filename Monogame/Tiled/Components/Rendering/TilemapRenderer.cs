using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.Tiled.Components.Rendering
{
    public class TilemapRenderer : RenderComponent
    {

        private Tilemap tilemap;
        protected override void Startup()
        {
            tilemap = GetComponent<Tilemap>();

            base.Startup();
        }

        protected override void Draw()
        {
            foreach (var item in tilemap.Tiles)
            {
                item.Draw();
            }            
        }
    }
}
