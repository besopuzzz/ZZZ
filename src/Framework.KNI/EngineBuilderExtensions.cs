using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZZZ.Framework.Assets;
using ZZZ.Framework.Extensions.SystemComponents;
using ZZZ.Framework.Rendering;
using ZZZ.Framework.Updating;

namespace ZZZ.Framework.KNI
{
    public static class EngineBuilderExtensions
    {

        public static EngineBuilder UseKNISystemHandler(this EngineBuilder gameBuilder, Game game, GetSystemHandler getSystemHandler = null)
        {
            ArgumentNullException.ThrowIfNull(game);

            var handler = new KNISystemHandler(game, getSystemHandler);

            gameBuilder.UseSystemHandler(handler);

            game.Components.Add(handler);

            return gameBuilder;
        }

        public static EngineBuilder UseKNIStaticAssetManager(this EngineBuilder gameBuilder, GameServiceContainer serviceProvider)
        {
            gameBuilder.UseStaticAssetManager<IAssetProvider>(new KNIAssetProvider(serviceProvider, "Content"));

            return gameBuilder;
        }


        public static EngineBuilder RegisterKNIRenderManagerService(this EngineBuilder gameBuilder, GraphicsDevice graphicsDevice)
        {
            gameBuilder.RegisterService<IRenderManager>(new KNIRenderManager(graphicsDevice));

            return gameBuilder;
        }

        public delegate IGameComponent GetSystemHandler(System system);

        private class KNISystemHandler : IGameComponent, IEngineHandler, IDisposable
        {
            public Game Game => game;

            private GetSystemHandler getSystemHandlerDelegate;
            private IEngine system;
            private Game game;

            public KNISystemHandler(Game game, GetSystemHandler getSystemHandler)
            {
                this.game = game;
                getSystemHandlerDelegate = getSystemHandler;
            }

            void IEngineHandler.Reception(System system)
            {
                switch (system)
                {
                    case ISystemUpdater systemUpdater:
                        game.Components.Add(new KNIUpdater(systemUpdater, game));
                        break;
                    case ISystemRenderer systemRenderer:
                        game.Components.Add(new KNIRenderer(systemRenderer, game));
                        break;
                    default:
                        var gameComponent = getSystemHandlerDelegate?.Invoke(system as System);

                        if (gameComponent != null)
                            game.Components.Add(gameComponent);

                        break;
                }

            }

            void IEngineHandler.Departure(System system)
            {
                var systems = game.Components.Where(x => x is IKNISystem).Cast<IKNISystem>().Where(x => x.System == system).Cast<IGameComponent>();

                foreach (var sys in systems)
                {
                    game.Components.Remove(sys);
                }
            }

            public void Initialize()
            {
                system = Services.Get<IEngine>();
                system.Run();
            }

            void IDisposable.Dispose()
            {
                system?.Dispose();
            }
        }
    }
}
