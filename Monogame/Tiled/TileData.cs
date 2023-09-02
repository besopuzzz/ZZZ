using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Monogame.Rendering.Content;

namespace ZZZ.Framework.Monogame.Tiled
{
    public struct TileData
    {
        public Color Color { get; set; }
        public SpriteEffects SpriteEffect { get; set; }
        public Sprite Sprite { get; set; }
    }
}
