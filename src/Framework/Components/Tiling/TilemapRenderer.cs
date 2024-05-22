using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Core.Rendering;

namespace ZZZ.Framework.Components.Tiling
{
    public sealed class TilemapRenderer : Component, ITilemap
    {
        public int Order
        {
            get => order;
            set
            {
                if(order == value) return;

                order = value;

                foreach (var item in cache.Values)
                {
                    item.Order = order;
                }
            }
        }
        public SortLayer Layer
        {
            get => layer;
            set
            {
                if (layer == value) return;

                layer = value;

                foreach (var item in cache.Values)
                {
                    item.Layer = layer;
                }
            }
        }

        private int order;
        private SortLayer layer =  SortLayer.Layer0;
        private Dictionary<Point, SpriteRenderer> cache = new Dictionary<Point, SpriteRenderer>();

        protected override void OnEnabledChanged()
        {
            foreach (var item in cache)
            {
                item.Value.Enabled = Enabled;
            }

            base.OnEnabledChanged();
        }

        void ITilemap.Add(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if(tile is IRenderTile render)
            {
                var renderComponent = container.AddComponent(new SpriteRenderer());
                renderComponent.Layer = layer;
                renderComponent.Enabled = Enabled;
                renderComponent.Order = Order;

                cache.Add(position, renderComponent);
            }
        }

        void ITilemap.Remove(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is IRenderTile render)
            {
                var renderComponent = container.GetComponent<SpriteRenderer>();
                container.RemoveComponent(renderComponent);

                cache.Remove(position);
            }
        }

        void ITilemap.SetData(GameObject container, ITile tile, Point position, Tilemap tilemap)
        {
            if (tile is IRenderTile render)
            {
                var renderComponent = cache[position];

                TileRenderData tileRenderData = new TileRenderData();
                render.GetRenderingData(position, tilemap, ref tileRenderData);

                renderComponent.Sprite = tileRenderData.Sprite;
                renderComponent.Color = tileRenderData.Color;
                renderComponent.SpriteEffect = tileRenderData.SpriteEffect;
            }
        }
    }
}
