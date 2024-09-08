using ZZZ.Framework.Core;

namespace ZZZ.Framework
{
    public class GameSettings
    {
        public IGameInstance Game { get; }

        public static GameSettings Instance { get; private set; }

        public GameSettings(GameManager  gameManager) 
        {
            if (Instance == null)
                Instance = this;

            Game = gameManager.Game;
        }
    }
}
