namespace ZZZ.Framework
{
    public sealed class Scene : GameContainer
    {
        [ContentSerializerIgnore]
        internal Engine Root => root;

        private Engine root;

        internal void Initialize(Engine root)
        {
            this.root = root;
        }

        protected override void Dispose(bool disposing)
        {
            root = null;

            base.Dispose(disposing);
        }

    }
}