using nkast.Aether.Physics2D.Dynamics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZZZ.Framework.Components.Physics.Aether.Components
{
    [RequiredComponent<Transformer>]
    internal sealed class BodyComponent : Component
    {
        internal Body Body { get; } = new Body();

        public const float PixelsPerMeter = 64f;

        private Transformer transformer;

        public void Attach(Collider collider)
        {
            Body.Add(collider.Fixture);
        }

        public void Detach(Collider collider)
        {
            Body.Remove(collider.Fixture);
        }

        protected override void Awake()
        {
            transformer = GetComponent<Transformer>();
            UpdatePosition();

            base.Awake();
        }

        internal void UpdatePosition()
        {
            Body.Position = transformer.World.Position / transformer.World.Scale / PixelsPerMeter;
            Body.Rotation = transformer.World.Rotation;
        }

        internal void UpdateTransformer()
        {
            Transform2D world = new Transform2D();

            world.Position = Body.Position * PixelsPerMeter * transformer.World.Scale;
            world.Scale = transformer.World.Scale;
            world.Rotation = Body.Rotation;

            transformer.World = world;
        }
    }
}
