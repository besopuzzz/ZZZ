using ZZZ.Framework.Components;
using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    /// <summary>
    /// Представляет интерфейс базового регистратора.
    /// </summary>
    public interface IRegistrar
    {
        /// <summary>
        /// Тип компонентов.
        /// </summary>
        Type Target { get; }
        internal GameManager GameManager { get; set; }
        internal void RegistrationObject(IComponent component);
        internal void DeregistrationObject(IComponent component);
    }
}
