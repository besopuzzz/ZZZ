using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using nkast.Aether.Physics2D.Dynamics;
using ZZZ.Framework.Monogame.FarseerPhysics.Diagnostics.Components;

namespace ZZZ.Framework.Monogame.FarseerPhysics.Components
{
    internal class WorldRegistrar : MonogameRegistrar<IBody>
    {
        [ContentSerializerIgnore]
        public World World => world;
        public bool Debug { get; set; }

        private WorldRenderer drawer;
        private World world;

        public WorldRegistrar()
        {
            world = new World(Vector2.Zero.ToAether());
        }

        protected override void Startup()
        {
            drawer = new WorldRenderer();
            drawer.Startup(Game, this);

            base.Startup();
        }

        protected override void Shutdown()
        {
            drawer.Shoutdown();

            base.Shutdown();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                drawer.Dispose();
            }

            drawer = null;
            world = null;

            base.Dispose(disposing);
        }

        protected override void Reception(IBody component)
        {
            component.World = World;

            base.Reception(component);
        }

        protected override void Departure(IBody component)
        {
            component.World = null;

            base.Departure(component);
        }

        protected override void OnEnabledChanged()
        {
            World.Enabled = Enabled;

            base.OnEnabledChanged();
        }

        protected override void Update(GameTime gameTime)
        {
            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (Debug)
                drawer.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
