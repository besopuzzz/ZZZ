using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Monogame.Content;

namespace ZZZ.Framework.Monogame.Rendering.Content
{
    public sealed class Sprite : Asset
    {
        public Rectangle? Source { get; private set; }
        public Vector2 Origin { get; set; }
        internal List<Sprite> Sprites { get; } = new List<Sprite>();
        internal Texture2D Texture => texture;

        public Sprite this[int index]
        {
            get 
            {
                return Sprites[index];
            }
        }

        private Texture2D texture;

        internal Sprite()
        {

        }

        internal Sprite(Texture2D texture, Rectangle? source, Vector2 origin)
        {
            this.texture = texture;
            Origin = origin;
            Source = source;
        }

        public Sprite CreateSub(Rectangle source, Vector2 origin)
        {
            Sprite sprite = new Sprite();
            sprite.texture = Texture;
            sprite.Origin = origin;
            sprite.Source = source;
            sprite.Name = Name + $"_{Sprites.Count}";

            Sprites.Add(sprite);

            return sprite;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                texture.Dispose();
            }

            texture = null;

            base.Dispose(disposing);
        }
    }


}
