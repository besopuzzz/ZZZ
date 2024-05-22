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
            if (instance == null)
                throw new Exception("SceneLoader not work before create instance GameManager!");

            if (scene is null)
                throw new ArgumentNullException(nameof(scene));

            instance.scene = scene;

            scene.GameManager = instance.manager;

            if (instance.manager.Initialized)
                ((GameObject)scene).Awake();
        }
    }
}
