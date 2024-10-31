using ZZZ.Framework.Components;

namespace ZZZ.Framework.Core
{
    public abstract class RootComponent : Component
    {
        protected IGameInstance GameInstance => InternalGameInstance;

        internal IGameInstance InternalGameInstance;

        public int AwakeOrder { get; }

        public RootComponent()
        {

        }

        public RootComponent(int awakeOrder)
        {
            AwakeOrder = awakeOrder;
        }
    }
}
