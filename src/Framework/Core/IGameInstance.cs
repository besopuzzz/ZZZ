namespace ZZZ.Framework.Core
{
    public interface IGameInstance
    {
        GameComponentCollection Components { get; }
        GameServiceContainer Services { get; }
    }
}
