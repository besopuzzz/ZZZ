using ZZZ.Framework.Assets;

namespace ZZZ.Framework.Rendering.Assets
{
    public sealed class Sprite : Asset
    {
        [ContentSerializer]
        public Rectangle? Source { get; }

        [ContentSerializer]
        public Vector2 Origin { get; }
        public Sprite this[int index]
        {
            get
            {
                return Sprites[index];
            }
        }
        internal List<Sprite> Sprites { get; } = new List<Sprite>();
        public Texture2D Texture => texture;


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
            Sprite sprite = new Sprite(Texture, source, origin);
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

        public static explicit operator Texture2D(Sprite sprite)
        {
            return sprite.texture;
        }
    }


}
