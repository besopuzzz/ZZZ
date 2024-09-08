using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.Collections.Generic;

namespace ZZZ.KNI.Content.Pipeline
{
    public class SpriteContent
    {
        public class Frame
        {
            public Rectangle Bounds { get; set; }
            public Vector2 Origin { get; set; }
        }
        public List<Frame> Frames { get; set; }
        public TextureContent Texture { get; set; }
    }

}
