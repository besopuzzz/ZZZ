using ZZZ.Framework.Assets;
using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core
{
    public class GameManager : IGameComponent
    {
        public IGameInstance Game { get;  }
        public IReadOnlyList<IRegistrar> Registrars => settings.Registrars;

        public static GameManager Instance => intance;

        internal bool Initialized => initialized;

        private static GameManager intance;
        private readonly GameManagerSettings settings;
        private bool initialized;

        private GameManager()
        {

        }

        public GameManager(IGameInstance game, GameManagerSettings managerSettings)
        {
            if (intance == null)
                intance = this;
            else throw new InvalidOperationException("GameManager already create instance!");

            Game = game;

            settings = managerSettings.Clone();

            _ = new AssetManager(this);
            _ = new GameSettings(this);
            _ = new SceneLoader(this);

            Game.Components.Add(this);

            foreach (var item in Registrars)
            {
                item.GameManager = this;

                Game.Components.Add(item as IGameComponent);
            }
        }

        internal static void RegistrationComponent<T>(T component) where T : IComponent
        {
            foreach (var item in intance.Registrars)
            {
                item.RegistrationObject(component);
            }
        }

        internal static void UnregistrationComponent<T>(T component) where T : IComponent
        {
            foreach (var item in intance.Registrars)
            {
                item.DeregistrationObject(component);
            }
        }

        protected virtual void Initialize()
        {

        }

        void IGameComponent.Initialize()
        {
            settings.InternalInitialize(this);

            var scene = SceneLoader.CurrentScene;

            if (scene != null)
            {
                ((GameObject)scene).Awake();
            }

            initialized = true;
        }
    }
}
