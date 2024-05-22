namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет класс с реализованным методом <see cref="Dispose()"/>.Только для наследования и удобства.
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        private bool disposedValue;

        /// <summary>
        /// Освобождает компонент от управляемых и неуправляемых ресурсов. Метод вызывается автоматически перед завершением приложения или сборщиком мусора.
        /// </summary>
        /// <param name="disposing">если true, то освобождение вызвали вручную. Если false, то освобождение вызвал метод завершения.</param>
        /// <remarks>Освобождайте ресурсы от медиа-ресурсов (спрайты, звуки и т.д.) при значение true вызвав resource.Dispose(). При значении false назначайте 
        /// ресурсам null ссылку.</remarks>
        protected virtual void Dispose(bool disposing)
        {
        }

        ~Disposable()
        {
            if (disposedValue)
                return;

            disposedValue = true;
            Dispose(disposing: false);
        }

        /// <summary>
        /// Освобождает компонент от управляемых ресурсов. Метод вызывается автоматически перед завершением приложения.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (disposedValue)
                return;

            disposedValue = true;
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
