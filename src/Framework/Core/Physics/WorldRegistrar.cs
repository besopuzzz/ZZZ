using Microsoft.Xna.Framework.Graphics;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Components.Rendering;
using ZZZ.Framework.Core.Physics;
using ZZZ.Framework.Core.Registrars;

namespace ZZZ.Framework.Physics.Components
{
    public class WorldRegistrar : BaseRegistrar<IBody>, IAnyRegistrar<IBody>
    {
        public bool Debug { get; set; }

        [ContentSerializerIgnore]
        public World World => world;

        private World world;
        private List<IBody> bodies;
        private static World instance;

        public WorldRegistrar()
        {
            world = new World(Vector2.Zero);
            
            instance = world;

            bodies = new List<IBody>();
        }

        public static RaycastResult Raycast(Vector2 start, Vector2 end)
        {
            return Raycast(start, end, ColliderLayer.All);
        }
        public static RaycastResult Raycast(Vector2 start, Vector2 end, ColliderLayer layer)
        {
            RaycastResult raycastResult = new RaycastResult();
            raycastResult.Start = start;
            raycastResult.End = end;
            raycastResult.Point = end;

            instance?.RayCast((fixture, point, normal, fraction) =>
            {
                var collider = fixture.Tag as Collider;

                if (!layer.HasFlag(collider.Layer))
                    return -1;

                raycastResult.Collider = collider;
                raycastResult.Point = point * IRigidbody.PixelsPerMeter;
                raycastResult.Normal = normal;
                raycastResult.Fraction = fraction * IRigidbody.PixelsPerMeter;

                return fraction;
            }, start / IRigidbody.PixelsPerMeter, end / IRigidbody.PixelsPerMeter);

            return raycastResult;
        }

        private static float RaycastDelegate(Fixture fixture, Vector2 point, Vector2 normal,float fraction)
        {
            if (fixture.Tag is Collider collider)
                collider.Owner.GetComponent<SpriteRenderer>().Color = Color.Red;

            return fraction;
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (var item in bodies)
            {
                item.UpdateBody();
            }

            World.Step(gameTime.ElapsedGameTime);

            foreach (var item in bodies)
            {
                item.UpdateTransformer();
            }


            base.Update(gameTime);
        }


        void IAnyRegistrar<IBody>.Reception(IBody body)
        {
            world.Add(body.Body);
            bodies.Add(body);
        }

        void IAnyRegistrar<IBody>.Departure(IBody body)
        {
            world.Remove(body.Body);
            bodies.Remove(body);    
        }

    }
}
