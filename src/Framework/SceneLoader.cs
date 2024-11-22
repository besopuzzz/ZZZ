using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    public sealed class SceneLoader : Disposable
    {
        internal static SceneLoader Instance
        {
            get
            {
                if (instance == null)
                    throw new InvalidOperationException("SceneLoader not initialized!");

                return instance;
            }
        }

        private static SceneLoader instance;
        private Engine root;

        internal SceneLoader(Engine root)
        {
            if (instance != null)
                throw new InvalidOperationException("SceneLoader already initialize!");

            instance = this;

            this.root = root;
        }

        protected override void Dispose(bool disposing)
        {
            root = null;
            instance = null;

            base.Dispose(disposing);
        }

        public static void Load(Scene scene)
        {
            if(scene == null)
                throw new ArgumentNullException(nameof(scene));

            Instance.root.AddGameObject(scene);
        }
    }
}
