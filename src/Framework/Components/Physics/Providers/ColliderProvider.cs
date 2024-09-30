namespace ZZZ.Framework.Components.Physics.Providers
{
    public abstract class ColliderProvider : IColliderProvider
    {
        public virtual Vector2 Offset { get => offset; set => offset = value; }
        public virtual bool IsTrigger { get => isTrigger; set => isTrigger = value; }
        public virtual ColliderLayer Layer { get => layer; set => layer = value; }
        public virtual float Friction { get => friction; set => friction = value; }
        public virtual float Restitution { get => restitution; set => restitution = value; }
        public virtual bool Enabled { get => enabled; set => enabled = value; }

        private bool enabled = true;
        private Vector2 offset;
        private bool isTrigger = false;
        private ColliderLayer layer = ColliderLayer.All;
        private float density = 1f;
        private float friction = 1f;
        private float restitution = 0f;

        public event ColliderEvent ColliderEnter;
        public event ColliderEvent ColliderExit;


        protected void InvokeEnter(ICollider sender, ICollider collider)
        {
            ColliderEnter?.Invoke(sender, collider);
        }
        protected void InvokeExit(ICollider sender, ICollider collider)
        {
            ColliderExit?.Invoke(sender, collider);
        }
    }
}
