using ZZZ.Framework.Monogame.Updating.Components;

namespace ZZZ.Framework.Monogame.Updating.Extentions
{
    public static class SceneExtentions
    {
        public static Scene UseUpdating(this Scene scene)
        {
            scene.AddComponent(new UpdateRegistrar() );

            return scene;
        }
    }
}
