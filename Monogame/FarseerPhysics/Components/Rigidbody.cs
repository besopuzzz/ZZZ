using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using tainicom.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Monogame.FarseerPhysics.Components
{
    [RequireComponent(Type = typeof(Transformer))]
    public class Rigidbody : UpdateComponent, IBody
    {
        public Vector2 Velocity
        {
            get => body.LinearVelocity.ToXna();
            set => body.LinearVelocity = value.ToAether();
        }
        public float AngularVelocity
        {
            get => body.AngularVelocity;
            set => body.AngularVelocity = value;
        }
        public BodyType BodyType
        {
            get => body.BodyType;
            set => body.BodyType = value;
        }
        public float Mass
        {
            get => body.Mass;
            set
            {
                body.Mass = value;

            }
        }
        public float LinearDamping
        {
            get => body.LinearDamping;
            set => body.LinearDamping = value;
        }
        public float AngularDamping
        {
            get => body.AngularDamping;
            set => body.AngularDamping = value;
        }
        public float Inertia
        {
            get => body.Inertia;
            set => body.Inertia = value;
        }
        public bool IsBullet
        {
            get => body.IsBullet;
            set => body.IsBullet = value;
        }
        public bool FixedRotation
        {
            get => body.FixedRotation;
            set => body.FixedRotation = value;
        }
        public bool IgnoreGravity
        {
            get => body.IgnoreGravity;
            set => body.IgnoreGravity = value;
        }

        [ContentSerializerIgnore]
        World IBody.World
        {
            get => world;
            set => world = value;
        }

        private World world;
        private readonly Body body = new();
        private Transformer transformer;
        private bool flag = false;

        public Rigidbody()
        {
            body.BodyType = BodyType.Dynamic;
            body.Mass = 1f;
            body.LinearDamping = 3f;
            body.AngularDamping = 3f;
            body.IgnoreGravity = true;
        }

        protected override void Startup()
        {
            transformer = GetComponent<Transformer>();
            transformer.WorldChanged += Transformer_WorldChanged;

            body.Tag = Owner;
            body.LocalCenter = tainicom.Aether.Physics2D.Common.Vector2.Zero;
            body.Position = (transformer.World.Position / transformer.World.Scale).ToAether();
            body.Rotation = transformer.World.Rotation;
            body.Enabled = Enabled;

            world.Add(body);

            base.Startup();
        }
        protected override void Shutdown()
        {
            transformer.WorldChanged -= Transformer_WorldChanged;
            world.Remove(body);


            base.Shutdown();
        }

        protected override void RegistrationComponents()
        {
            RegistrationComponent<IBody>(this);

            base.RegistrationComponents();
        }

        protected override void UnregistrationComponents()
        {
            UnregistrationComponent<IBody>(this);

            base.UnregistrationComponents();
        }

        protected override void Update(GameTime gameTime)
        {
            Transform2D world = new(body.Position.ToXna() * transformer.World.Scale, transformer.World.Scale, body.Rotation);

            flag = true;
            body.LocalCenter = Vector2.Zero.ToAether();
            transformer.Local = world / transformer.World * transformer.Local;
            flag = false;

            base.Update(gameTime);
        }
        protected override void OnEnabledChanged()
        {
            body.Enabled = Enabled;

            base.OnEnabledChanged();
        }

        private void Transformer_WorldChanged(ITransformer sender, Transform2D args)
        {
            if (flag)
                return;

            body.Position = (transformer.World.Position / transformer.World.Scale).ToAether();
            body.Rotation = transformer.World.Rotation;
        }
        internal Body GetBody()
        {
            return body;
        }
    }
}
