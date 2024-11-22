using ZZZ.Framework.Components;

namespace ZZZ.Framework
{
    public abstract class System : Component
    {
        internal System Next { get; set; }

        /// <summary>
        /// Вызывает событие, когда компоненты добавляется на сцену, а так же вызывает событие у следующей системы через базовый метод.
        /// </summary>
        /// <param name="components">Перечислитель компонентов, добавленные на сцену.</param>
        protected virtual void Input(IEnumerable<Component> components)
        {
            if (components.Any())
                Next?.Input(components);
        }

        /// <summary>
        /// Вызывает событие, когда компонент удаляется со сцены, а так же вызывает событие у следующей системы через базовый метод.
        /// </summary>
        /// <param name="component">Компонент, удаляемый со сцены.</param>
        protected virtual void Output(Component component)
        {
            if (component != null)
                Next?.Output(component);
        }

        internal void InternalInput(IEnumerable<Component> components)
        {
            Input(components);
        }

        internal void InternalOutput(Component component)
        {
            Output(component);
        }
    }
}
