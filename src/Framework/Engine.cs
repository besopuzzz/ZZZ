using System.ComponentModel.Design;
using ZZZ.Framework.Designing.UnityStyle.Systems;

namespace ZZZ.Framework
{
    internal sealed class Engine : GameContainer, IEngine
    {
        private readonly EventedList<Scene> scenes;
        private readonly Services services;
        private readonly IEngineHandler systemHandler;

        private static Engine instance;
        private System firstSystem;

        internal Engine(ServiceContainer container, List<Type> components, IEngineHandler handler)
        {
            if (instance != null)
                throw new InvalidOperationException("Engine already initialize!");

            instance = this;

            scenes = [];

            container.AddService(typeof(IEngine), this);
            container.AddService(typeof(SceneLoader), new SceneLoader(this));

            services = new Services(container);

            systemHandler = handler;

            firstSystem = AddComponent(typeof(AwakeShutdownSystem)) as System; // AwakeShutdown system forever be first.

            var componentAnalyzer = firstSystem;

            for (int i = 0; i < components.Count; i++)
            {
                componentAnalyzer.Next = AddComponent(components[i]) as System;

                componentAnalyzer = componentAnalyzer.Next;
            }
        }

        protected override void OnSceneComponentAdded<T>(T component)
        {
            firstSystem.InternalInput(Enumerable.Repeat(component, 1));
        }

        protected override void OnSceneComponentShutdowned<T>(T component)
        {
            firstSystem.InternalOutput(component);
        }

        protected override void OnGameObjectAdding<T>(T container)
        {
            if(container is Scene scene)
                scene.Initialize(this);

            base.OnGameObjectAdding(container);
        }

        protected override void OnGameObjectRemoved<T>(T container)
        {
            base.OnGameObjectRemoved(container);

            if (container is Scene scene)
                scene.Initialize(null);
        }

        public override IEnumerable<GameObject> GetGameObjects()
        {
            return scenes.SelectMany(x => x.GetGameObjects());
        }

        public override IEnumerable<GameObject> FindGameObjects(Predicate<GameObject> predicate)
        {
            List<GameObject> gameObjects = new List<GameObject>();

            foreach (var gameObject in GetGameObjects())
            {
                gameObjects.AddRange(FindGameObjectsRecursiveDown(gameObject, predicate));
            }

            return gameObjects;
        }

        public void Run()
        {
            base.Awake();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                for (int i = 0; i < scenes.Count; i++)
                {
                    (scenes[i] as IDisposable)?.Dispose();
                }

                ((IDisposable)services).Dispose();
            }

            scenes.Clear();

            base.Dispose(disposing);
        }

        protected override void OnComponentAdding<T>(T component)
        {
            systemHandler?.Reception(component);

            base.OnComponentAdding(component);
        }

        protected override void OnComponentRemoved<T>(T component)
        {
            base.OnComponentRemoved(component);

            systemHandler?.Departure(component);
        }

        private IEnumerable<GameObject> FindGameObjectsRecursiveDown(GameObject container, Predicate<GameObject> predicate)
        {
            var containers = container.GetGameObjects();
            var findeds = containers.Where(x => predicate.Invoke(x));

            foreach (var item in containers)
            {
                findeds.Union(FindGameObjectsRecursiveDown(item, predicate));
            }

            return findeds;
        }

    }
}
