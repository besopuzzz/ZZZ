namespace ZZZ.Framework
{
    public interface IEngine : IDisposable
    {
        bool Enabled { get; }
        void Run();
    }
}