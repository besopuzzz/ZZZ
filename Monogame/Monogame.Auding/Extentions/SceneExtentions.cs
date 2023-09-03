using ZZZ.Framework.Monogame.Auding.Components;

namespace ZZZ.Framework.Monogame.Auding.Extentions
{
    public static class SceneExtentions
    {
        public static Scene UseAuding(this Scene scene)
        {
            scene.AddComponent(new SoundRegistrar());

            return scene;
        }
    }
}
