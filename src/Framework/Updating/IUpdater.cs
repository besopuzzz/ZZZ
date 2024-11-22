namespace ZZZ.Framework.Updating
{
    /// <summary>
    /// Представляет основной интерфейс обновляемого компонента.
    /// </summary>
    public interface IUpdater
    {
        bool Enabled { get; }

        /// <summary>
        /// Вызывает метод обновления компонента примерно 60 раз в секунду.
        /// </summary>
        /// <param name="dt">Количество пройденного времени с последнего кадра в миллисекундах.</param>
        /// <remarks>Используйте метод, например, для изменения положения объекта или подсчета пройденного расстояния.</remarks>
        void Update(TimeSpan time);
    }
}
