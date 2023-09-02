namespace ZZZ.Framework.Monogame.Content
{
    public class Asset : IAsset, IDisposable
    {
        [ContentSerializer]
        public string Name { get; internal set; } = "";

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
        }

        ~Asset()
        {
            if (disposed)
                return;

            Dispose(disposing: false);
            disposed = true;
        }

        public void Dispose()
        {
            if (disposed)
                return;

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
            disposed = true;
        }

    }
}
