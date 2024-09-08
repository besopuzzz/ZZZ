using ZZZ.Framework.Components.Transforming;
using ZZZ.Framework.Core;
using ZZZ.Framework.Core.Rendering;

namespace ZZZ.Framework.Components
{
    public class UITransformer : Transformer
    {
        public Vector2 Size
        {
            get
            {
                return size;
            }

            set
            {
                if (size == value)
                    return;

                size = value;
                CalculateBounds();
            }
        }

        public Rectangle Bounds => bounds;

        private Vector2 size = Vector2.Zero;
        private Rectangle bounds = new Rectangle();
        private UITransformer UIParent => Parent as UITransformer;

        protected override void Awake()
        {
            base.Awake();

            CalculateBounds();
        }

        protected override void OnWorldChanged(Transformer sender, Transform2D args)
        {
            base.OnWorldChanged(sender, args);

            CalculateBounds();
        }

        private void CalculateBounds()
        {
            bounds = World.CreateBounds(size);
        }
    }
}

