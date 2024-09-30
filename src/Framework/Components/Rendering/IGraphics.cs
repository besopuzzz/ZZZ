
namespace ZZZ.Framework.Core.Rendering.Components
{
    /// <summary>
    /// Представляет интерфейс, описывающий компонент для рисования объектов на экран.
    /// </summary>
    public interface IGraphics : IComponent
    {
        /// <summary>
        /// Слой, на котором действует компонент.
        /// </summary>
        SortLayer Layer { get; }
    }
}
