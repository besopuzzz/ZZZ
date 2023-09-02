using ZZZ.Framework.Monogame.FarseerPhysics.Components;

namespace ZZZ.Framework.Monogame.FarseerPhysics
{
    public class ColliderEventArgs : EventArgs
    {
        public Collider Other { get; }
        public bool Ignore { get; set; }

        public ColliderEventArgs(Collider other)
        {
            Other = other;
        }
    }
}
