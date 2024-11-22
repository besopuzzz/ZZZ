using ZZZ.Framework.Assets;
using ZZZ.Framework.Components;
using ZZZ.Framework.Designing.UnityStyle.Systems;
using ZZZ.Framework.Extensions.SystemComponents;
using ZZZ.Framework.Rendering;

namespace ZZZ.Framework.Extensions
{
    public static class KNIGameBuilderExtensions
    {
        /// <summary>
        /// Used static AssetManager class where KNI ContentManager provider.
        /// </summary>
        /// <param name="gameBuilder">Instance of builder.</param>
        /// <returns></returns>
        public static EngineBuilder UseStaticAssetManager(this EngineBuilder gameBuilder, GameServiceContainer serviceProvider)
        {
            UseStaticAssetManager(gameBuilder, new KNIAssetProvider(serviceProvider, "Content"));

            return gameBuilder;
        }

        public static EngineBuilder UseStaticAssetManager<T>(this EngineBuilder gameBuilder, T provider)
            where T : IAssetProvider
        {
            ArgumentNullException.ThrowIfNull(provider);

            gameBuilder.RegisterService(new AssetManager(provider));

            return gameBuilder;
        }


        public static EngineBuilder UseKNIUnityStyle(this EngineBuilder gameBuilder, Game game)
        {
            gameBuilder
                .UseStaticAssetManager(game.Services)
                .UseKNISystemHandler(game)
                .RegisterKNIRenderManagerService(game.GraphicsDevice)
                .UseSystemBottom<KNIStartStopSystem>()
                .UseSystemBottom<EnabledDisabledSystem>()
                .UseKNIUpdaterSystem()
                .UseKNIRendererSystem();

            return gameBuilder;
        }

        public static EngineBuilder RegisterKNIRenderManagerService(this EngineBuilder gameBuilder, GraphicsDevice graphicsDevice)
        {
            gameBuilder.RegisterService<IRenderManager>(new KNIRenderManager(graphicsDevice));

            return gameBuilder;
        }

        public static EngineBuilder UseKNIRendererSystem(this EngineBuilder gameBuilder)
        {
            gameBuilder
                .UseSystemBottom<KNISystemRenderer>();

            return gameBuilder;
        }

        public static EngineBuilder UseKNIUpdaterSystem(this EngineBuilder gameBuilder)
        {
            gameBuilder
                    .UseSystemBottom<KNiSystemUpdater>();

            return gameBuilder;
        }

        public static EngineBuilder UseKNISystemHandler(this EngineBuilder gameBuilder, Game game)
        {
            ArgumentNullException.ThrowIfNull(game);

            var handler = new KNISystemHandler(game);

            gameBuilder.UseSystemHandler(handler);

            game.Components.Add(handler);
            
            return gameBuilder;
        }

        private class KNISystemHandler : IGameComponent, IEngineHandler, IDisposable
        {
            private IEngine system;
            private Game game;

            public KNISystemHandler(Game game)
            {
                this.game = game;
            }

            void IEngineHandler.Reception(Component component)
            {
                if (component is IGameComponent gameComponent)
                    game.Components.Add(gameComponent);

            }

            void IEngineHandler.Departure(Component component)
            {
                if (component is IGameComponent gameComponent)
                    game.Components.Remove(gameComponent);
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
