namespace ZZZ.Framework.Core.Rendering.Components
{
    /// <summary>
    /// Представляет интерфейс, описывающий компонент камеры.
    /// </summary>
    public interface ICamera : IGraphics
    {
        /// <summary>
        /// Матрица проекции.
        /// </summary>
        Matrix Projection { get; }
        bool IsMain { get; }
        void UpdateMatrix();
        void Render(SpriteBatch spriteBatch);
    }
}
