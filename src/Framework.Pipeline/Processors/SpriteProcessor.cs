using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.ComponentModel;
using static ZZZ.KNI.Content.Pipeline.SpriteContent;

namespace ZZZ.KNI.Content.Pipeline.Processors
{
    [ContentProcessor(DisplayName = "Sprite Processor - ZZZ")]
    public class SpriteProcessor : ContentProcessor<TextureContent, SpriteContent>
    {
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }

        public SpritePivot Pivot { get; set; }

        public enum SpritePivot
        {
            Center = 0,
            LeftTop = 1,
            Top = 2,
            RightTop = 3,
            Left = 4,
            Right = 5,
            LeftBottom = 6,
            Bottom = 7,
            RightBottom = 8
        }


        [DefaultValue(typeof(Color), "255,0,255,255")]
        public virtual Color ColorKeyColor { get; set; } = new Color(255, 0, 255, 255);

        [DefaultValue(true)]
        public virtual bool ColorKeyEnabled { get; set; } = true;

        public virtual bool GenerateMipmaps { get; set; }

        [DefaultValue(true)]
        public virtual bool PremultiplyAlpha { get; set; } = true;

        public virtual bool ResizeToPowerOfTwo { get; set; }

        public virtual bool MakeSquare { get; set; }

        public virtual TextureProcessorOutputFormat TextureFormat { get; set; }


        public override SpriteContent Process(TextureContent input, ContentProcessorContext context)
        {
            var texture = input.Faces[0][0];

            List<Frame> rectangles = new List<Frame>();

            if (FrameWidth > 0 & FrameHeight > 0)
            {
                var textureSize = new Point(texture.Width, texture.Height);
                var textureBounds = new Rectangle(Point.Zero, textureSize);

                int widthMaxStep = textureSize.X / FrameWidth;
                int heightMaxStep = textureSize.Y / FrameHeight;

                for (int y = 0; y < heightMaxStep; y++)
                {
                    for (int x = 0; x < widthMaxStep; x++)
                    {
                        var rectangle = new Rectangle(FrameWidth * x, FrameHeight * y, FrameWidth, FrameHeight);
                        rectangles.Add(new Frame() { Bounds = rectangle, Origin = OriginFromPivot(FrameWidth, FrameHeight, Pivot) });
                    }
                }

            }

            TextureProcessor textureProcessor = new TextureProcessor();
            textureProcessor.TextureFormat = TextureFormat;
            textureProcessor.ResizeToPowerOfTwo = ResizeToPowerOfTwo;
            textureProcessor.PremultiplyAlpha = PremultiplyAlpha;
            textureProcessor.ColorKeyColor = ColorKeyColor;
            textureProcessor.ColorKeyEnabled = ColorKeyEnabled;
            textureProcessor.GenerateMipmaps = GenerateMipmaps;
            textureProcessor.MakeSquare = MakeSquare;

            return new SpriteContent() { Frames = rectangles, Texture = textureProcessor.Process(input, context) };
        }



        private Vector2 OriginFromPivot(int width, int height, SpritePivot spritePivot)
        {
            var origin = new Vector2();

            if (Pivot == SpritePivot.Center)
                return new Vector2(width, height) / 2;

            if (Pivot == SpritePivot.Top | Pivot == SpritePivot.Bottom)
                origin.X = width / 2;
            else if (Pivot == SpritePivot.RightTop | Pivot == SpritePivot.Right | Pivot == SpritePivot.RightBottom)
                origin.X = width;

            if (Pivot == SpritePivot.Left | Pivot == SpritePivot.Right)
                origin.Y = height / 2;
            else if (Pivot == SpritePivot.LeftBottom | Pivot == SpritePivot.Bottom | Pivot == SpritePivot.RightBottom)
                origin.Y = height;

            return origin;
        }
    }


}
