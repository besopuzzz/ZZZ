using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Monogame.Asseting.Assets;
using ZZZ.Framework.Monogame.Rendering.Content;
using ZZZ.Framework.Monogame.Tiling.Components;

namespace ZZZ.Framework.Monogame.Tiling.Assets
{
    public class Tile : Asset
    {
        public Sprite Sprite { get; set; }

        [ContentSerializer(Optional = true)]
        public Color Color { get; set; } = Color.White;

        [ContentSerializer(Optional = true)]
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;

        [ContentSerializerIgnore]
        public object Tag { get; set; }

        internal Tileset Tileset { get; set; }

        internal Tile()
        {

        }

        public Tile(Sprite sprite)
        {
            Sprite = sprite;
            Color = Color.White;
            SpriteEffect = SpriteEffects.None;
        }

        public Tile(Sprite sprite, Color color, SpriteEffects spriteEffect) : this(sprite)
        {
            Color = color;
            SpriteEffect = spriteEffect;
        }

        public virtual Tile GetTile(Point position, Tilemap tilemap)
        {
            return this;
        }

        public virtual void Refresh(Point position, Tilemap tilemap)
        {

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Sprite.Dispose();
            }

            Sprite = null;

            base.Dispose(disposing);
        }
    }
}
