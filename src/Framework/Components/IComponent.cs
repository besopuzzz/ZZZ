using System.ComponentModel;

namespace ZZZ.Framework.Components
{
    /// <summary>
    /// Представляет основной интерфейс для игровых компонентов.
    /// </summary>
    public interface IComponent : IDisposable
    {
        /// <summary>
        /// Получает значение, включен или выключен компонент.
        /// <see href="true"/> - объект включен; иначе - выключен.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Вызывает событие, когда значение <see cref="Enabled"/> было изменено.
        /// </summary>
        event EventHandler<EventArgs> EnabledChanged;

        GameObject Owner { get; }
    }
}
