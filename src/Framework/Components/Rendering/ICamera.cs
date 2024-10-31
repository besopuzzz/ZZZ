using ZZZ.Framework.Components;
using ZZZ.Framework.Core.Rendering.Entities;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.Core.Rendering.Components
{
    /// <summary>
    /// Представляет интерфейс, описывающий компонент камеры.
    /// </summary>
    public interface ICamera : IComponent
    {
        /// <summary>
        /// Матрица проекции.
        /// </summary>
        Matrix Projection { get; }
        Matrix View { get; }
        Matrix World { get; }
        SortLayer LayerMask { get; }
        bool IsMain { get; }
        void UpdateMatrix();
        void Apply(RenderContext renderContext);
    }
}
