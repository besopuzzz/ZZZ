using System.ComponentModel.Design;

namespace ZZZ.Framework
{
    public class EngineBuilder
    {
        private IEngineHandler systemHandler;
        private readonly ServiceContainer container;
        private readonly List<Type> components;

        public EngineBuilder()
        {
            container = new ServiceContainer();
            components = new List<Type>();
        }

        #region Systems

        public EngineBuilder UseSystemAfter<T, T2>()
            where T : System
            where T2 : System, new()
        {

            var index = components.FindIndex(x => x is T);

            if (index >= 0)
                index++;
            else index = components.Count;

            components.Insert(index, typeof(T2));

            return this;
        }

        public EngineBuilder UseSystemBefore<T, T2>()
            where T : System
            where T2 : System, new()
        {
            var index = components.FindIndex(x => x is T);

            if (index == -1)
                index = 0;

            components.Insert(index, typeof(T2));

            return this;
        }

        public EngineBuilder UseSystemTop<T>()
            where T : System, new()
        {
            components.Insert(0, typeof(T));

            return this;
        }

        public EngineBuilder UseSystemBottom<T>()
            where T : System, new()
        {
            components.Add(typeof(T));

            return this;
        }


        #endregion

        public EngineBuilder UseSystemHandler<T>(T handler)
            where T : IEngineHandler
        {
            ArgumentNullException.ThrowIfNull(handler);

            systemHandler = handler;

            return this;
        }

        public EngineBuilder RegisterService<T>(T instance)
        {
            container.AddService(typeof(T), instance);

            return this;
        }

        public IEngine Build()
        {
            return new Engine(container, components, systemHandler);
        }
    }
}
