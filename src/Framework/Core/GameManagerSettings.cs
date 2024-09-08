using ZZZ.Framework.Assets;
using ZZZ.Framework.UserInterfacing;

namespace ZZZ.Framework.Core
{
    public class GameManagerSettings
    {
        public EventedList<IRegistrar> Registrars { get; }

        public GameManagerSettings() 
        {
            Registrars = new EventedList<IRegistrar>();
        }

        protected virtual void Initialize(GameManager gameManager)
        {

        }

        internal void InternalInitialize(GameManager gameManager)
        {
            Initialize(gameManager);
        }

        public virtual GameManagerSettings Clone()
        {
            var clone = new GameManagerSettings();

            clone.Registrars.AddRange(Registrars);

            return clone;
        }
    }
}
