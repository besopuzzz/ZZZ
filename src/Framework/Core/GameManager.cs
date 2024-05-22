using ZZZ.Framework.Assets;

namespace ZZZ.Framework.Core
{
    public class GameManager : Disposable
    {
        public Game Game { get; }
        public bool Initialized => initialized;
        public IReadOnlyList<IRegistrar> Registrars => registrars;

        private readonly List<IRegistrar> registrars = new List<IRegistrar>();
        private bool initialized = false;

        protected GameManager(Game game)
        {
            Game = game;
        }

        public static GameManagerBuilder StartBuild(Game game)
        {
            return new GameManagerBuilder(new GameManager(game));
        }

        public static GameManagerBuilder StartBuild(GameManager gameManager)
        {
            return new GameManagerBuilder(gameManager);
        }

        internal void UseRegistrar(IRegistrar registrar)
        {
            ArgumentNullException.ThrowIfNull(registrar);

            if (initialized)
                throw new Exception("GameManager not support new registrars after initialize.");

            if (registrars.Contains(registrar))
                throw new Exception("Registrar already exist!");

            registrar.GameManager = this;

            registrars.Add(registrar);

            OnRegistrarAdded(registrar);
        }

        internal void RegistrationComponent<T>(T component) where T : IComponent
        {
            foreach (var item in registrars)
            {
                item.RegistrationObject(component);
            }
        }

        internal void UnregistrationComponent<T>(T component) where T : IComponent
        {
            foreach (var item in registrars)
            {
                item.DeregistrationObject(component);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var item in registrars)
                {
                    if (item is IDisposable disposable)
                        disposable.Dispose();
                }
            }

            registrars.Clear();

            base.Dispose(disposing);
        }

        protected virtual void OnRegistrarAdded(IRegistrar registrar)
        {

        }

        protected virtual void Initialize()
        {

        }

        internal void InternalInitialize()
        {
            if (initialized)
                return;

            foreach (var item in Registrars)
            {
                Game.Components.Add(item as IGameComponent);
            }

            Initialize();

            var scene = SceneLoader.CurrentScene;

            if (scene != null)
                ((GameObject)scene).Awake();

            initialized = true;
        }
    }
}
