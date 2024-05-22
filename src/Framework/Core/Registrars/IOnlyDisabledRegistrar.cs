namespace ZZZ.Framework.Core.Registrars
{
    public interface IOnlyDisabledRegistrar<T> : IRegistrar
        where T : IComponent
    {
        void DisabledReception(T component);
        void DisabledDeparture(T component);
    }
}
