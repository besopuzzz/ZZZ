using ZZZ.Framework.Monogame.Transforming;
using ZZZ.Framework.Monogame.Transforming.Components;
using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Monogame.Rendering.Components
{
    public class Camera : UpdateComponent
    {
        public RenderLayer Layer
        {
            get => layer;
            set
            {
                if (layer == value) return;

                RenderLayerEventArgs args = new RenderLayerEventArgs(value, layer, null);

                layer = value;
                LayerChanged?.Invoke(this, args);
            }
        }
        public event EventHandler<RenderLayerEventArgs> LayerChanged;
        public bool FixedRotation
        {
            get => ignoreZ;
            set
            {
                if (value == ignoreZ)
                    return;

                ignoreZ = value;

                if (Owner != null)
                    Build();
            }
        }
        public Vector2 Origin
        {
            get => origin;
            set
            {
                if (value == origin)
                    return;

                origin = value;

                if (Owner != null)
                    Build();
            }
        }
        public Matrix? Projection => matrix;

        private Matrix? matrix = Matrix.Identity;
        private bool ignoreZ = true;
        private Vector2 origin = Vector2.Zero;
        private Transformer transformer;
        private RenderLayer layer = RenderLayer.All;

        protected override void Startup()
        {
            transformer = GetComponent<Transformer>();
            //UpdateOrder = int.MinValue;

            base.Startup();
        }

        protected virtual Matrix BuildMatrix()
        {
            Transform2D local = transformer.Local;
            Transform2D world = transformer.Parent.World;

            float rotation = FixedRotation ? local.Rotation : transformer.World.Rotation;

            return  Matrix.CreateTranslation(new Vector3(-world.Position, 0f)) *
                    Matrix.CreateRotationZ(-rotation) *
                    Matrix.CreateScale(new Vector3(local.Scale, 1f)) *
                    Matrix.CreateTranslation(new Vector3(local.Position, 0)); 
        }

        protected override void OnEnabledChanged()
        {
            matrix = null;

            base.OnEnabledChanged();
        }

        protected override void Update(GameTime gameTime)
        {
            Build();

            base.Update(gameTime);
        }

        private void Build()
        {
            matrix = BuildMatrix();
        }
    }
}
