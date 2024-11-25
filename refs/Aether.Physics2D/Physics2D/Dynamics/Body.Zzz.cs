using nkast.Aether.Physics2D.Common;
using ZZZ.Framework;

namespace nkast.Aether.Physics2D.Dynamics
{
    public partial class Body
    {
        public const float PixelsPerMeter = 64f;

        private Transformer transformer;

        public Body(Transformer transformer) : this()
        {
            this.transformer = transformer;

            Position = (transformer.World.Position / transformer.World.Scale) / PixelsPerMeter;
            Rotation = transformer.World.Rotation;
        }

        private void ChangeTransformer()
        {
            Transform2D world = new(Position * PixelsPerMeter * transformer.World.Scale, transformer.World.Scale, Rotation);

            if (world == transformer.World)
                return;

            transformer.World = world;
        }
    }
}
