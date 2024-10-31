using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core.Registrars
{
    /// <summary>
    /// Представляет регистратор, который запускает <see cref="IStartupComponent.Startup"/> и/или останавливает <see cref="IStopComponent.Stop"/>
    /// компонент в зависимости от событий.
    /// </summary>
    /// <remarks>Например, запуск будет выполняться один раз после присоединения к контейнеру (в зависимости от <see cref="Component.Enabled"/>
    /// и всегда после события <see cref="Component.EnabledChanged"/> (если компонент все еще на сцене).
    /// Аналогично и для остановки компонента. Регистратор дает гарантию, что запуск будет вызван единожды до вызова остановки и наоборот.</remarks>
    public class InitializeRegistrar : BaseRegistrar<Component>, IOnlyEnabledRegistrar<Component>
    {
        private List<Component> components;

        /// <summary>
        /// Инициализирует новый экземпляр инициализирующего регистратора.
        /// </summary>
        public InitializeRegistrar()
        {
            components = new List<Component>();
        }

        protected override void Initialize()
        {
            while (components.Count > 0)
            {
                var component = components[0];

                InvokeStartup(component);

                components.RemoveAt(0);
            }

            base.Initialize();
        }

        void IOnlyEnabledRegistrar<Component>.EnabledDeparture(Component component)
        {
            if (Initialized)
                InvokeStop(component);
            else components.Remove(component);
        }

        void IOnlyEnabledRegistrar<Component>.EnabledReception(Component component)
        {
            if (Initialized)
                InvokeStartup(component);
            else components.Add(component);
        }

        /// <summary>
        /// Метод запуска компонента.
        /// </summary>
        /// <param name="component">Экземпляр компонента, который надо запустить.</param>
        protected virtual void InvokeStartup(Component component)
        {
            if (component is IStartupComponent startupComponent)
                startupComponent.Startup();
        }

        /// <summary>
        /// Метод остановки компонента.
        /// </summary>
        /// <param name="component">Экземпляр компонента, который надо остановить.</param>
        protected virtual void InvokeStop(Component component)
        {
            if (component is IStopComponent stopComponent)
                stopComponent.Stop();
        }
    }
}
