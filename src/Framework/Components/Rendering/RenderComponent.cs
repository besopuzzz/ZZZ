using ZZZ.Framework.Core.Rendering;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.Components.Rendering
{
    public abstract class RenderComponent : Component, IComparable<RenderComponent>
    {
        public int Order { get; set; }

        public SortLayer Layer { get; set; }

        internal RenderComponent UpNeighbor
        {
            get => upNeighbor;

            set
            {
                if (upNeighbor != null)
                    upNeighbor.downNeighbors.Remove(this);

                upNeighbor = value;

                if (value != null)
                    upNeighbor.downNeighbors.Add(this);
            }
        }

        internal virtual IList<RenderComponent> DownNeighbors => downNeighbors;

        private RenderComponent upNeighbor;
        private readonly List<RenderComponent> downNeighbors = new List<RenderComponent>();
        private readonly Comparer<int> comparer = Comparer<int>.Default;

        protected override void Awake()
        {
            SignalMessenger.SendToParents<RenderComponent>(this.Owner, this, (x, y) =>
            {
                UpNeighbor = x;

                foreach (var child in x.downNeighbors)
                {
                    if (child.Owner.IsParent(Owner))
                        child.UpNeighbor = this;
                }

                return true;
            });

            base.Awake();
        }

        protected override void Shutdown()
        {
            UpNeighbor = null;

            base.Shutdown();
        }

        protected virtual void Render(RenderContext renderContext)
        {

        }

        internal void InternalRender(RenderContext renderContext)
        {
            Render(renderContext);
        }

        int IComparable<RenderComponent>.CompareTo(RenderComponent other)
        {
            return comparer.Compare(Order, other.Order);
        }
    }
}
