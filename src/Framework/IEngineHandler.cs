using ZZZ.Framework.Components;

namespace ZZZ.Framework
{
    public interface IEngineHandler
    {
        void Reception(Component component);
        void Departure(Component component);
    }
}
