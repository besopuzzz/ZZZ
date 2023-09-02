using ZZZ.Framework.Monogame.Content;
using ZZZ.Framework.Monogame.Transforming.Components;

namespace ZZZ.Framework.Monogame
{
    public class Scene : Root<IMonogameController>, IGameComponent
    {
        private Game game;

        public Scene()
        {
            AddComponent<Transformer>(new Transformer());
        }

        public Scene(Game game) : this()
        {
            this.game = game;
            new AssetManager(game.Services);
        }

        public static Scene FromAsset(string name, Game game)
        {
            new AssetManager(game.Services);

            var scene = AssetManager.Instance.Load<Scene>(name);
            scene.game = game;

            return scene;
        }

        public static string ToAsset(Scene scene)
        {
            return AssetManager.Instance.Serialize(scene);
        }

        protected override void PrepairController(IMonogameController controller)
        {
            controller.Game = game;
            game.Components.Add(controller);

            base.PrepairController(controller);
        }

        protected override void DepairController(IMonogameController controller)
        {
            game.Components.Remove(controller);

            base.DepairController(controller);
        }

        void IGameComponent.Initialize()
        {
            Startup();
        }
    }
}