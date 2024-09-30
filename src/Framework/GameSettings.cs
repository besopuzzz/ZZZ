using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    public class GameSettings
    {
        public IGameInstance Game => gameManager.Game;
        public Point ScreenSize
        {
            get
            {
                if (graphicsDevice == null)
                    graphicsDevice = gameManager.Game.Services.GetService<IGraphicsDeviceService>().GraphicsDevice;

                return graphicsDevice.Viewport.Bounds.Size;
            }
            set
            {
                if (graphicsDevice == null)
                    graphicsDevice = gameManager.Game.Services.GetService<IGraphicsDeviceService>().GraphicsDevice;

                graphicsDevice.Viewport = new Viewport(0,0,value.X,value.Y);
            }
        }

        public static GameSettings Instance { get; private set; }

        private GameManager gameManager;
        private GraphicsDevice graphicsDevice;

        public GameSettings(GameManager  gameManager) 
        {
            if (Instance == null)
                Instance = this;

            this.gameManager = gameManager;

            graphicsDevice = gameManager.Game.Services.GetService<IGraphicsDeviceService>().GraphicsDevice;
        }
    }
}
