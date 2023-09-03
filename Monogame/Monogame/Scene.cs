 

namespace ZZZ.Framework.Monogame
{
    public class Scene : Root<IMonogameRegistrar>, IGameComponent
    {
        private Game game;

        public Scene()
        {
        }

        public Scene(Game game) : this()
        {
            this.game = game;
        }

        protected override void PrepairController(IMonogameRegistrar controller)
        {
            controller.Game = game;
            game.Components.Add(controller);

            base.PrepairController(controller);
        }

        protected override void DepairController(IMonogameRegistrar controller)
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