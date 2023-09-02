using Microsoft.Xna.Framework;

namespace ZZZ.Framework.Monogame.Transforming.Components
{
    public class Transformer : Component, ITransformer
    {
        public Transform2D Local
        {
            get => local;
            set
            {
                if (local == value)
                    return;

                local = value;
                LocalChanged?.Invoke(this, local);

                world = local * parent.World;
                WorldChanged?.Invoke(this, world);
            }
        }
        public Transform2D World
        {
            get => world;
            set
            {
                if (world == value)
                    return;

                local = value / world * local;
                LocalChanged?.Invoke(this, local);

                world = value;
                WorldChanged?.Invoke(this, world);
            }
        }
        public Transformer Parent => parent;

        ITransformer ITransformer.Parent => parent;

        public event TransformerEventHandler LocalChanged;
        public event TransformerEventHandler WorldChanged;

        private Transformer parent;
        private Transform2D local = new Transform2D();
        private Transform2D world = new Transform2D();

        private static readonly Transformer empty = new Transformer();

        public Transformer()
        {
            parent = empty;
        }
        public Transformer(Transform2D transform) : this()
        {
            local = transform;
        }

        public Transformer(Vector2 position) : this(position, Vector2.One, 0f)
        {

        }
        public Transformer(Vector2 position, Vector2 scale) : this(position, scale, 0f)
        {

        }
        public Transformer(float rotation) : this(Vector2.Zero, Vector2.One, rotation)
        {

        }
        public Transformer(float rotation, Vector2 scale) : this(Vector2.Zero, scale, rotation)
        {

        }
        public Transformer(float x, float y) : this(new Vector2(x,y))
        {

        }
        public Transformer(Vector2 position, float rotation) : this(position, Vector2.One, rotation)
        {

        }
        public Transformer(Vector2 position, Vector2 scale, float rotation) : this(new Transform2D(position, scale, rotation))
        {

        }


        protected override void Startup()
        {
            parent = Owner?.Owner?.GetComponent<Transformer>();

            if (parent == null)
                parent = empty;

            AddWorld();
            parent.WorldChanged += Parent_WorldChanged;

            base.Startup();
        }
        protected override void Shutdown()
        {
            parent.WorldChanged -= Parent_WorldChanged;
            RemoveWorld();

            base.Shutdown();
        }

        private void Parent_WorldChanged(ITransformer sender, Transform2D args)
        {
            world = local * parent.World;
            WorldChanged?.Invoke(this, world);
        }
        private void AddWorld()
        {
            world = local * parent.World;
        }
        private void RemoveWorld()
        {
            world = local / parent.World;
        }
        public void SetWorld(Transform2D world)
        {
            this.world = world;
        }
        public void SetLocal(Transform2D local)
        {
            this.local = local;
        }
    }
}
