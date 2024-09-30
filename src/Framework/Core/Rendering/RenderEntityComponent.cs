using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core.Rendering
{
    public class RenderEntityComponent : EntityComponent<RenderEntity, RenderEntityComponent, IRender>, IComparable<RenderEntityComponent>
    {
        public int Order => Component.Order;
        public SortLayer SortLayer => Component.Layer;

        private readonly Comparer<int> comparer = Comparer<int>.Default;

        public RenderEntityComponent(IRender component) : base(component)
        {

        }

        public virtual void Render(ICamera camera, SpriteBatch spriteBatch)
        {
            Component.Render(spriteBatch);
        }

        public override string ToString()
        {
            return $"Component: {Component}";
        }

        public int CompareTo(RenderEntityComponent other)
        {
            return comparer.Compare(Order, other.Order);
        }
    }
}
