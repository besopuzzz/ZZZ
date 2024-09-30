using ZZZ.Framework.Components.Physics.Providers;
using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;

namespace ZZZ.Framework.Components.Physics
{
    public delegate void ColliderEvent(ICollider sender, ICollider other);

    //public abstract class Collider<T> : Collider
    //    where T : Shape
    //{
    //    protected new T Shape => (T)base.Shape;

    //    protected Collider(T shape) : base(shape)
    //    {
    //    }
    //}

    //[RequireComponent(typeof(BodyController))]
    //public abstract class Collider : Component, ICollider
    //{
    //    public Vector2 Offset
    //    {
    //        get => offset;
    //        set
    //        {
    //            if (offset == value)
    //                return;

    //            var old = offset;

    //            offset = value;

    //            OnOffsetChanged(old, offset);
    //        }
    //    }
    //    public bool IsTrigger
    //    {
    //        get => fixture.IsSensor;
    //        set => fixture.IsSensor = value;
    //    }
    //    public ColliderLayer Layer
    //    {
    //        get => layer;
    //        set
    //        {
    //            layer = value;

    //            if (body != null)
    //                fixture.CollidesWith = (Category)(int)value;
    //        }
    //    }
    //    public float Friction
    //    {
    //        get => fixture.Friction;
    //        set => fixture.Friction = value;
    //    }

    //    /// <summary>
    //    /// Упругость материала.
    //    /// </summary>
    //    public float Restitution
    //    {
    //        get => fixture.Restitution;
    //        set => fixture.Restitution = value;
    //    }

    //    /// <summary>
    //    /// Плотность материала.
    //    /// </summary>
    //    public float Density
    //    {
    //        get => shape.Density;
    //        set => shape.Density = value;
    //    }

    //    protected virtual Shape Shape => shape;

    //    public event ColliderEvent ColliderEnter;
    //    public event ColliderEvent ColliderExit;

    //    private ColliderLayer layer = ColliderLayer.Cat1;
    //    private readonly Shape shape;
    //    private Fixture fixture;
    //    private Vector2 offset;
    //    private Body body;

    //    protected Collider(Shape shape)
    //    {
    //        fixture = (Fixture)Activator.CreateInstance(typeof(Fixture), true); 

    //        Type type = typeof(Fixture);

    //        type.GetProperty(nameof(fixture.Shape)).SetValue(fixture, shape);
    //        type.GetProperty(nameof(fixture.Proxies)).SetValue(fixture, new FixtureProxy[shape.ChildCount]);

    //        fixture.Tag = this;
    //        fixture.OnCollision = OnCollision;
    //        fixture.OnSeparation = OnSeparation;

    //        this.shape = shape;
    //    }

    //    private void OnSeparation(Fixture sender, Fixture other, Contact contact)
    //    {
    //        if (sender == fixture & sender.IsSensor)
    //            ColliderExit?.Invoke(this, other.Tag as Collider);

    //    }
    //    private bool OnCollision(Fixture sender, Fixture other, Contact contact)
    //    {
    //        if (sender == fixture & sender.IsSensor & contact.IsTouching & Enabled)
    //            ColliderEnter?.Invoke(this, other.Tag as Collider);

    //        return Enabled;
    //    }

    //    protected abstract void OnOffsetChanged(Vector2 oldOffset, Vector2 offset);

    //    void IPhysicBody.Attach(Body body)
    //    {
    //        this.body = body;
    //        body.Add(fixture);

    //        body.LocalCenter = Vector2.Zero;

    //        fixture.CollidesWith = (Category)(int)layer; // Layer change throw error if Body is null...
    //    }

    //    void IPhysicBody.Detach()
    //    {
    //        body.Remove(fixture);
    //        body = null;
    //    }
    //}

    [RequireComponent(typeof(Transformer))]
    public abstract class Collider<TProvider> : Component, ICollider
        where TProvider : class, IColliderProvider
    {
        public Vector2 Offset
        {
            get => Provider.Offset;
            set
            {
                Provider.Offset = value;
            }
        }
        public bool IsTrigger
        {
            get => Provider.IsTrigger;
            set
            {
                Provider.IsTrigger = value;
            }
        }
        public ColliderLayer Layer
        {
            get => Provider.Layer;
            set
            {
                Provider.Layer = value;
            }
        }
        public float Friction 
        { 
            get => Provider.Friction;
            set => Provider.Friction = value; 
        }
        public float Restitution 
        { 
            get => Provider.Restitution; 
            set => Provider.Restitution = value; 
        }
        public override bool Enabled 
        {
            get => Provider.Enabled;
            set => Provider.Enabled = value; 
        }

        public event ColliderEvent ColliderEnter
        {
            add => Provider.ColliderEnter += value;
            remove => Provider.ColliderEnter -= value;
        }
        public event ColliderEvent ColliderExit
        {
            add => Provider.ColliderExit += value;
            remove => Provider.ColliderExit -= value;
        }

        protected TProvider Provider { get; }

        IColliderProvider ICollider.ColliderProvider => Provider;

        protected Collider()
        {
            if(GameManager.Instance?.Game.Services.GetService<IPhysicsProvider>() != null)
                Provider = GameManager.Instance.Game.Services.GetService<IPhysicsProvider>().CreateColliderProvider<TProvider>(this);
            else Provider = CreateEmptyProvider();
        }

        protected abstract TProvider CreateEmptyProvider();
    }
}
