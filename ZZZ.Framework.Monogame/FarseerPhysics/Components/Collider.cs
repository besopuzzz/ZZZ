using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;

namespace ZZZ.Framework.Monogame.FarseerPhysics.Components
{
    public abstract class Collider : Component, IBody
    {
        public Vector2 Offset
        {
            get => offset;
            set
            {
                if (offset == value)
                    return;

                var old = offset;
                offset = value;
                OnOffsetChanged(old, value);
            }
        }
        public PhysicsMaterial Material
        {
            get => material;
            set
            {
                if (material == value)
                    return;

                material = value;
                OnMaterialChanged();
            }
        }
        public bool IsTrigger
        {
            get => isTrigger;
            set
            {
                if (isTrigger == value)
                    return;

                isTrigger = value;
                OnIsTriggerChanged();
            }
        }
        public ColliderLayer Layer
        {
            get
            {
                return (ColliderLayer)Enum.ToObject(typeof(ColliderLayer), (int)Category);
            }
            set
            {
                var newValue = (Category)Enum.ToObject(typeof(Category), (int)value);

                if (newValue == Category)
                    return;

                Category = newValue;
                OnLayerChanged();
            }
        }

        protected Body Body = new Body();
        protected Category Category = Category.Cat1;

        private Rigidbody Rigidbody;
        private PhysicsMaterial material = new PhysicsMaterial();
        private bool isTrigger = false;
        private Body oldBody;
        private World World;
        private Vector2 offset = Vector2.Zero;
        private Transformer transformer;

        World IBody.World { get => World; set => World = value; }

        protected override void Startup()
        {
            RegistrationComponent<IBody>(this);

            Owner.ComponentAdded += GameObject_ComponentAdded;
            Owner.ComponentRemoved += GameObject_ComponentRemoved;

            transformer = GetComponent<Transformer>();
            transformer.WorldChanged += Transformer_WorldChanged;


            oldBody = Body;
            Body.Position = (transformer.World.Position / transformer.World.Scale).ToAether();
            Body.Rotation = transformer.World.Rotation;
            Body.Tag = Owner;

            Create();

            World.Add(Body);
            Body.Enabled = Enabled;

            Rigidbody = GetComponent<Rigidbody>();

            if (Rigidbody != null)
            {
                Rebuild(Rigidbody.GetBody(), true);
            }
            base.Startup();
        }
        protected override void Shutdown()
        {
            World.Remove(Body);
            UnregistrationComponent<IBody>(this);

            Owner.ComponentAdded -= GameObject_ComponentAdded;
            Owner.ComponentRemoved -= GameObject_ComponentRemoved;

            transformer.WorldChanged -= Transformer_WorldChanged;

            base.Shutdown();
        }
        protected override void OnEnabledChanged()
        {
            if (Body != null)
                Body.Enabled = Enabled;

            base.OnEnabledChanged();
        }

        protected abstract void Create();
        protected abstract void Clear();

        protected virtual void Rebuild()
        {
            Rebuild(null, false);
        }
        protected virtual void OnMaterialChanged()
        {

        }
        protected virtual void OnIsTriggerChanged()
        {

        }
        protected virtual void OnOffsetChanged(Vector2 oldValue, Vector2 newValue)
        {

        }
        protected virtual void OnLayerChanged()
        {

        }

        private void GameObject_ComponentRemoved(IContainer sender, IComponent component)
        {
            if (component is Rigidbody & Rigidbody != null)
            {
                Rigidbody rigidbody = component as Rigidbody;

                if (Rigidbody != rigidbody)
                    return;

                Rebuild(oldBody, false);

                Body.Position = (transformer.World.Position / transformer.World.Scale).ToAether();
                Body.Rotation = transformer.World.Rotation;

                World.Add(Body);
                Rigidbody = null;
            }
        }
        private void GameObject_ComponentAdded(IContainer sender, IComponent component)
        {
            if (component is Rigidbody & Rigidbody == null)
            {
                Rigidbody = component as Rigidbody;

                Rebuild(Rigidbody?.GetBody(), true);
            }
        }
        private void Rebuild(Body swap = null, bool remove = false)
        {
            Clear();

            if (swap != null)
            {
                if (remove)
                    World.Remove(Body);
                Swap(swap);
            }

            Create();
        }
        private void Swap(Body newBody)
        {
            oldBody = Body;
            Body = newBody;
            Body.Enabled = Enabled;
        }
        private void Transformer_WorldChanged(ITransformer sender, Transform2D args)
        {
            if (Rigidbody != null)
                return;

            Body.Position = (transformer.World.Position / transformer.World.Scale).ToAether();
            Body.Rotation = transformer.World.Rotation;
        }
    }
}
