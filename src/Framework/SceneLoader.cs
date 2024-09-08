using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    public sealed class SceneLoader
    {
        public static Scene CurrentScene => instance?.scene;

        private static SceneLoader instance;
        private Scene scene;
        private GameManager manager;

        internal SceneLoader(GameManager gameManager)
        {
            if (instance == null)
                instance = this;

            manager = gameManager;
        }

        public static void Load(string name)
        {

        }

        public static void Load(Scene scene)
        {
            ArgumentNullException.ThrowIfNull(scene);

            instance.scene = scene;

            if (scene?.GameManager == null)
                return;

            scene.GameManager = instance.manager;

            if (!scene.GameManager.Initialized)
                return;

            ((GameObject)scene).Awake();
        }
    }
}
