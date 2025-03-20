using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;

namespace ZZZ.Framework.Physics.Aether.Components
{
    [RequiredComponent<Transformer>]
    internal sealed class BodyComponent : Component
    {
        internal const float PixelsPerMeter = 64f;

        internal IEnumerable<Collider> Colliders => colliders;

        internal Body Body { get; } = new Body();

        private Transformer transformer;
        private List<Collider> colliders = new List<Collider>();

        private List<BodyComponent> childs = new List<BodyComponent>();

        internal void Attach(BodyComponent child)
        {
            childs.Add(child);
        }

        internal void Detach(BodyComponent child)
        {
            childs.Remove(child);
        }


        internal void Attach(Collider collider)
        {
            var fixture = collider.Fixture;

            Body.Add(fixture);

            fixture.CollidesWith = (Category)(int)collider.Layer; // Layer change throw error if Body is null...
        }

        internal void Detach(Collider collider)
        {
            Body.Remove(collider.Fixture);
        }

        protected override void OnCreated()
        {
            transformer = GetComponent<Transformer>();

            Body.Position = transformer.World.Position / transformer.World.Scale / PixelsPerMeter;
            Body.Rotation = transformer.World.Rotation;

            base.OnCreated();
        }

        internal void UpdatePosition()
        {
            if (Body.BodyType == BodyType.Dynamic)
                return;

            if (!transformer.HasChanges)
                return;

            Body.Position = transformer.World.Position / transformer.World.Scale / PixelsPerMeter;
            Body.Rotation = transformer.World.Rotation;


            foreach (var child in childs)
            {
                child.UpdatePosition();
            }
        }

        internal void UpdateTransformer()
        {
            if (Body.BodyType != BodyType.Static)
            {
                Transform2D world = new Transform2D();

                world.Position = Body.Position * PixelsPerMeter * transformer.World.Scale;
                world.Scale = transformer.World.Scale;
                world.Rotation = Body.Rotation;

                transformer.World = world;
            }

            foreach (var child in childs)
            {
                child.UpdateTransformer();
            }
        }
    }
}
