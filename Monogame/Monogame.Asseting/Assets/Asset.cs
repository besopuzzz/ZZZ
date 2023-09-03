using Microsoft.Xna.Framework.Content;

namespace ZZZ.Framework.Monogame.Asseting.Assets
{
    public class Asset : IAsset
    {
        [ContentSerializer]
        public string Name { get; set; } = "";

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
