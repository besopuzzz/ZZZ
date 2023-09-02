using ZZZ.Framework.Monogame.Tiled.Content;

namespace ZZZ.Framework.Monogame.Tiled
{
    internal class TileAnimator
    {
        public AnimatedTile AnimatedTile { get; }

        private List<TileRenderData> tiles = new List<TileRenderData>();
        private float currentTime = 0f;
        private int currentSprite = 0;

        public TileAnimator()
        {

        }

        public TileAnimator(AnimatedTile animatedTile)
        {
            AnimatedTile = animatedTile;
        }


        public bool Unregister(TileRenderData tileRenderData)
        {
            if (Contains(tileRenderData))
            {
                tiles.Remove(tileRenderData);
            }
            return tiles.Count == 0;
        }

        public void Register(TileRenderData tileRenderData)
        {
            if (Contains(tileRenderData))
                return;

            tiles.Add(tileRenderData);
            tileRenderData.ChangeSprite(AnimatedTile.Sprites[currentSprite]);
        }

        public void Reset()
        {
            currentTime = AnimatedTile.StartOffset;
            currentSprite = 0;
            ChangeSprite();
        }

        public bool Contains(TileRenderData tileRenderData)
        {
            return tiles.Contains(tileRenderData);
        }

        public void Update(float dt)
        {
            currentTime += dt;

            if (currentTime >= AnimatedTile.Duration)
            {
                currentTime = 0;
                currentSprite++;
                currentSprite %= AnimatedTile.Sprites.Count;
                ChangeSprite();
            }
        }

        private void ChangeSprite()
        {
            foreach (var item in tiles)
            {
                item.ChangeSprite(AnimatedTile.Sprites[currentSprite]);
            }
        }
    }
}
