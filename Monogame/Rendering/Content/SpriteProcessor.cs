using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using ZZZ.Framework.Monogame.Rendering.Content;

namespace ZZZ.Framework.Monogame.Rendering.Content
{
    [ContentProcessor(DisplayName = "Sprite Processor - ZZZ")]
    public class SpriteProcessor : ContentProcessor<Texture2DContent, SpriteContent>
    {
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int OffsetStartX { get; set; }
        public int OffsetStartY { get; set; }
        public int OffsetEndX { get; set; }
        public int OffsetEndY { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }

        public override SpriteContent Process(Texture2DContent input, ContentProcessorContext context)
        {
            var size = new Point(FrameWidth, FrameHeight);
            var startOffset = new Point(OffsetStartX, OffsetStartY);
            var endOffset = new Point(OffsetEndX, OffsetEndY);
            var texture = input.Faces[0][0];
            Point textureSize = new Point(texture.Width, texture.Height);
            Rectangle textureBounds = new Rectangle(Point.Zero, textureSize);

            int widthMaxStep = textureSize.X / (size.X + startOffset.X + endOffset.X);
            int heightMaxStep = textureSize.Y / (size.Y + startOffset.Y + endOffset.Y);

            Point endOffset2 = Point.Zero;
            List<Frame> rectangles = new List<Frame>();

            for (int y = 0; y < heightMaxStep; y++)
            {
                for (int x = 0; x < widthMaxStep; x++)
                {
                    Rectangle rectangle = new Rectangle((startOffset.X + size.X) * x, (startOffset.Y + size.Y) * y, size.X, size.Y);
                    rectangle.Location += endOffset2;
                    endOffset2 = endOffset;

                    rectangles.Add(new Frame() { Bounds = Rectangle.Intersect(textureBounds, rectangle), Origin = new Vector2(OriginX, OriginY) });
                }
            }

            return new SpriteContent() { Frames = rectangles, Texture = input };
        }

    }

    public class SpriteContent
    {
        public List<Frame> Frames { get; set; }
        public Texture2DContent Texture { get; set; }
    }

    public class Frame
    {
        public Rectangle Bounds { get; set; }
        public Vector2 Origin { get; set; }
    }

    [ContentTypeWriter]
    public class SpriteAssetWriter : ContentTypeWriter<SpriteContent>
    {
        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(Sprite).AssemblyQualifiedName;
        }
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(SpriteReader).AssemblyQualifiedName;
        }

        protected override void Write(ContentWriter output, SpriteContent value)
        {
            output.WriteObject(value.Texture);
            output.Write(value.Frames.Count);

            for (int i = 0; i < value.Frames.Count; i++)
            {
                output.WriteObject(value.Frames[i].Bounds);
                output.WriteObject(value.Frames[i].Origin);
            }
        }
    }
}
