using ZZZ.Framework.Monogame.Asseting.Components;

namespace ZZZ.Framework.Monogame.Asseting.Extentions
{
    public static class SceneExtentions
    {
        public static Scene UseAsseting(this Scene scene)
        {
            scene.AddComponent(new AssetRegistrar());

            return scene;
        }
    }
}
