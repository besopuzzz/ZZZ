using ZZZ.Framework.Assets.Tiling;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Rendering.Assets;

namespace ZZZ.Framework.Components.Tiling
{
    internal class TileAnimator
    {
        public float Duration { get; set; }
        public float StartOffset { get; set; }
        public Sprite[] Sprites { get; set; }
        public List<SpriteRenderer> Renderers { get; } = new List<SpriteRenderer>();
        public IAnimatedTile Tile { get; set; }

        private float currentTime = 0f;
        private int currentSprite = 0;

        public void SetData(Point position, Tilemap tilemap)
        {
            TileAnimationData tileAnimationData = new TileAnimationData();

            Tile.GetAnimationData(position, tilemap, ref tileAnimationData);

            Duration = tileAnimationData.Duration;
            StartOffset = tileAnimationData.StartOffset;
            Sprites = tileAnimationData.Sprites;
        }

        public void Reset()
        {
            currentTime = StartOffset;
            currentSprite = 0;
            ChangeSprite();
        }

        public void Update(float dt)
        {
            currentTime += dt;

            if (currentTime >= Duration)
            {
                currentTime = 0;
                currentSprite++;
                currentSprite %= Sprites.Length;
                ChangeSprite();
            }
        }

        protected virtual void ChangeSprite()
        {
            Renderers.ForEach(x => x.Sprite = Sprites[currentSprite]);
        }

    }
}
