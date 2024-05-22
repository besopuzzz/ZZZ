namespace ZZZ.Framework.Core.Registrars
{
    public interface IAcceptingChangesRegistrar<T> : IAnyRegistrar<T>
        where T : IComponent
    {
        void AcceptingChanges(IComponent component);
    }
}
