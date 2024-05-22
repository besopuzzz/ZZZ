namespace ZZZ.Framework.Assets
{
    public interface IAssetProvider
    {
        T Load<T>(string path);
        T LoadPrefab<T>(string path) where T : GameObject;
    }
}
