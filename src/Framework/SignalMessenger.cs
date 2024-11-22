using ZZZ.Framework.Components;

namespace ZZZ.Framework
{
    /// <summary>
    /// Предоставляет статический класс для отправки сигналов для других компонентов.
    /// </summary>
    public class SignalMessenger
    {
        /// <summary>
        /// Делегат отправки сигналов до найденных компонентов типа <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Тип компонента.</typeparam>
        /// <param name="target">Экземпляр найденного компонента.</param>
        /// <param name="data">Пользовательский параметр.</param>
        /// <returns>Если <c>true</c>, то выполнение прервется. Если необходимо продолжить, то <c>false</c>.</returns>
        public delegate bool SignalFunction<T>(T target, object data);

        /// <summary>
        /// Отправляет сигнал всем компонентам указанного игрового объекта.
        /// </summary>
        /// <typeparam name="T">Тип компонента.</typeparam>
        /// <param name="target">Экземпляр найденного компонента.</param>
        /// <param name="data">Пользовательский параметр.</param>
        /// <param name="action">Функция отправки сигнала.</param>
        /// <returns>Если <c>true</c>, то выполнение прервано. Иначе - выполнение закончено.</returns>
        public static bool Send<T>(GameObject target, object data, SignalFunction<T> action)
        {
            if (target != null)
                return target.SendSignal<T>(data, action);

            return false;
        }

        /// <summary>
        /// Отправляет сигнал всем потомкам указанного игрового объекта.
        /// Обратите внимание, что глубина отправки выполняется с помощью функции отправки сообщений <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="T">Тип компонента.</typeparam>
        /// <param name="target">Экземпляр найденного компонента.</param>
        /// <param name="data">Пользовательский параметр.</param>
        /// <param name="action">Функция отправки сигнала.</param>
        public static void SendToChilds<T>(GameObject target, object data, SignalFunction<T> action)
        {
            if (target != null)
                target.SendSignalToChilds<T>(data, action);
        }

        /// <summary>
        /// Отправляет сигнал всем предкам указанного игрового объекта.
        /// Обратите внимание, что глубина отправки выполняется с помощью функции отправки сообщений <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="T">Тип компонента.</typeparam>
        /// <param name="target">Экземпляр найденного компонента.</param>
        /// <param name="data">Пользовательский параметр.</param>
        /// <param name="action">Функция отправки сигнала.</param>
        public static void SendToParents<T>(GameObject target, object data, SignalFunction<T> action)
        {
            if (target != null)
                target.SendSignalToParents<T>(data, action);
        }
    }
}
