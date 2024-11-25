namespace ZZZ.Framework.Rendering.Assets.Pipeline
{
    public class SpriteReader : ContentTypeReader<Sprite>
    {
        protected override  Sprite Read(ContentReader input, Sprite existingInstance)
        {
            var texture = input.ReadObject<Texture2D>();
            var count = input.ReadInt32();

            Sprite sprite = new Sprite(texture, null, Vector2.Zero);
            sprite.Name = input.AssetName;

            for (int i = 0; i < count; i++)
            {
                var bounds = input.ReadObject<Rectangle>();
                var origin = input.ReadObject<Vector2>();

                sprite.CreateSub(bounds, origin);
            }

            return sprite;
        }
    }
}
