using System.ComponentModel;
using ZZZ.Framework.Core.Rendering.Components;

namespace ZZZ.Framework.Core
{
    public interface ISystem<TComponent, TEntity>
        where TComponent : IComponent
        where TEntity : BaseEntity<TComponent, TEntity>
    {
        BaseEntity<TComponent, TEntity> Process(TComponent component);
    }
}
