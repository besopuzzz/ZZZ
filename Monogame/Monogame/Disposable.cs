namespace ZZZ.Framework.Monogame
{
    public abstract class Disposable : IDisposable
    {
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
        }

        ~Disposable()
        {
            if (disposedValue)
                return;

            Dispose(disposing: false);
            disposedValue = true;
        }

        public void Dispose()
        {
            if (disposedValue)
                return;

            Dispose(disposing: true);
            GC.SuppressFinalize(this);
            disposedValue = true;
        }
    }
}
