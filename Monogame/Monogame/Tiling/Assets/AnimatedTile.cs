using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Tiling.Components;

namespace ZZZ.Framework.Monogame.Tiling.Assets
{
    public class AnimatedTile : Tile
    {
        public List<Sprite> Sprites { get; set; }
        public float Duration { get; set; }

        [ContentSerializer(Optional = true)]
        public float StartOffset { get; set; }

        internal AnimatedTile()
        {

        }

        public AnimatedTile(float duration, float startOffset, List<Sprite> sprites) : base(sprites[0])
        {
            Duration = duration;
            StartOffset = startOffset;
            Sprites = sprites;
        }

        public AnimatedTile(float duration, float startOffset, params Sprite[] sprites) : base(sprites[0])
        {
            Duration = duration;
            StartOffset = startOffset;
            Sprites = sprites.ToList();
        }
        public override Tile GetTile(Point position, Tilemap tilemap)
        {
            return this;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in Sprites)
                {
                    item.Dispose();
                }
            }

            Sprites = null;

            base.Dispose(disposing);
        }
    }
}
