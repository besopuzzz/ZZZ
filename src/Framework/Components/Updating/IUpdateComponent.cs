namespace ZZZ.Framework.Components.Updating
{
    /// <summary>
    /// Представляет основной интерфейс обновляемого компонента.
    /// </summary>
    public interface IUpdateComponent : IComponent
    {
        /// <summary>
        /// Вызывает метод обновления компонента примерно 60 раз в секунду.
        /// </summary>
        /// <param name="gameTime">Количество пройденного времени с последнего кадра в миллисекундах.</param>
        /// <remarks>Используйте метод, например, для изменения положения объекта или подсчета пройденного расстояния.</remarks>
        void Update(GameTime gameTime);
    }
}
