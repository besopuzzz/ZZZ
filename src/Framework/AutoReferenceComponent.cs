using ZZZ.Framework.Components;

namespace ZZZ.Framework
{
    public abstract class AutoReferenceComponent
    {
        public Type Target { get; }

        protected AutoReferenceComponent(Type type)
        {
            Target = type;
        }

        internal abstract void Connect(Component caller, Component other);

        internal abstract void Disconnect(Component caller, Component other);
    }

    public abstract class AutoReferenceComponent<T> : AutoReferenceComponent
        where T : Component, new()
    {
        public AutoReferenceComponent() : base(typeof(T))
        {

        }

        protected abstract void Connect(Component caller, T required);

        protected abstract void Disconnect(Component caller, T required);

        internal override void Connect(Component caller, Component required)
        {
            if (required is T typical)
                Connect(caller, typical);
        }

        internal override void Disconnect(Component caller, Component required)
        {
            if (required is T typical)
                Disconnect(caller, typical);
        }
    }
}
