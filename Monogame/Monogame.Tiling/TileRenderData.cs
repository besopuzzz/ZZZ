using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Monogame.Rendering;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Tiling.Components;
using ZZZ.Framework.Monogame.Tiling.Assets;
using ZZZ.Framework.Monogame.Transforming;

namespace ZZZ.Framework.Monogame.Tiling
{
    internal sealed class TileRenderData : IDisposable
    {

        [ContentSerializer(SharedResource = true)]
        public Tile BaseTile { get; private set; }

        [ContentSerializer]
        public Point Position { get; private set; }
        public Transform2D Local { get; private set; }
        public Transform2D World { get; private set; }

        private Sprite sprite;
        private Color color;
        private SpriteEffects spriteEffect;
        private bool disposedValue;

        public TileRenderData()
        {

        }

        public TileRenderData(Point position, Tile baseTile)
        {
            BaseTile = baseTile;
            Position = position;
        }

        public void Refresh(Tilemap tilemap)
        {
            var tile = BaseTile.GetTile(Position, tilemap);

            sprite = tile.Sprite;
            color = tile.Color;
            spriteEffect = tile.SpriteEffect;

            tilemap.ClearAnimationTale(this);

            AnimatedTile animatedTile = tile as AnimatedTile;

            if (animatedTile != null)
                tilemap.AddAnimatedTile(this, animatedTile);

            Local = new Transform2D(tilemap.GetPositionFromPoint(Position)); 
        }

        public void Refresh(TileRenderData other)
        {
            BaseTile = other.BaseTile;
        }

        public void ChangeSprite(Sprite newSprite)
        {
            sprite = newSprite;
        }

        public void UpdateWorld(Transform2D world)
        {
            World = Local * world;
        }

        public void Draw()
        {
            Renderer.DrawSprite(sprite, World, color, spriteEffect, 0f);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                sprite?.Dispose();
                BaseTile?.Dispose();
            }

            sprite = null;
            BaseTile = null;

            disposedValue = true;
        }

        ~TileRenderData()
        {
            if (disposedValue)
                return;

            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            if (disposedValue)
                return;

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
