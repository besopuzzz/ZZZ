namespace ZZZ.Framework.Updating
{
    public interface IUpdateHandler : IUpdater
    {
        void AddToQueue(IUpdater updater);
        void RemoveFromQueue(IUpdater updater);
    }
}
