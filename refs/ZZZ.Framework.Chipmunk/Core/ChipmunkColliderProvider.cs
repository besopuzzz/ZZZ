using ChipmunkSharp;
using Microsoft.Xna.Framework;
using System.Net.Http.Headers;
using ZZZ.Framework.Components.Physics;
using ZZZ.Framework.Components.Physics.Providers;

namespace ZZZ.Framework.Chipmunk.Core
{
    internal class ChipmunkPolygonColliderProvider : ChipmunkColliderProvider, IPolygonColliderProvider
    {
        public List<Vector2> Vertices
        {
            get => vertices;
            set
            {
                vertices = value;

                shape2.SetVerts(vertices.Count, vertices.Select(x => new cpVect(x.X, x.Y)).ToArray());
            }
        }

        private List<Vector2> vertices;

        cpPolyShape shape2 => shape as cpPolyShape;

        public override Vector2 Offset
        {
            get
            {
                return offset;
            }
            set
            {
                var razn = value - offset;

                offset = value;
            }
        }

        private Vector2 offset;

        public ChipmunkPolygonColliderProvider(ICollider collider, cpBody cpBody) : base(collider, cpPolyShape.BoxShape(cpBody, 50f,50f,1f))
        {
        }

    }
    internal class ChipmunkCircleColliderProvider : ChipmunkColliderProvider, ICircleColliderProvider
    {
        public override Vector2 Offset
        {
            get
            {
                var offset = shape2.GetOffset();
                return new Vector2(offset.x, offset.y); 
            }
            set => shape2.SetOffset(new cpVect(value.X, value.Y));
        }
        public virtual float Radius
        {
            get => shape2.GetRadius();
            set
            {
                shape2.SetRadius(value);
            }
        }

        cpCircleShape shape2 => shape as cpCircleShape;

        public ChipmunkCircleColliderProvider(ICollider collider, cpBody cpBody) : base(collider, new cpCircleShape(cpBody, 100f, cpVect.Zero))
        {
        }
    }

    public abstract class ChipmunkColliderProvider : IColliderProvider
    {
        public abstract Vector2 Offset { get; set; }
        public bool IsTrigger { get => shape.GetSensor(); set => shape.SetSensor(value); }
        public ColliderLayer Layer { get => (ColliderLayer)shape.filter.mask; set => shape.filter.mask = (int)value; }
        public float Friction { get => shape.GetFriction(); set => shape.SetFriction(value); }
        public float Restitution { get => shape.GetElasticity(); set => shape.SetElasticity(value); }
        public bool Enabled { get => enabled; set => enabled = value; }

        public event ColliderEvent ColliderEnter;
        public event ColliderEvent ColliderExit;

        private bool enabled = true;
        protected cpShape shape;

        public ChipmunkColliderProvider(ICollider collider, cpShape cpShape)
        {
            shape = cpShape;
        }

        public void Attach(cpSpace cpSpace)
        {
            cpSpace.AddShape(shape);
        }
    }
}
