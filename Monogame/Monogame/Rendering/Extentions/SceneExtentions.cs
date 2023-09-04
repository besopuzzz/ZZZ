using ZZZ.Framework.Monogame.Rendering.Components;

namespace ZZZ.Framework.Monogame.Rendering.Extentions
{
    public static class SceneExtentions
    {
        public static Scene UseRenderingSystem(this Scene scene)
        {
            scene.AddComponent(new RenderRegistrar());

            return scene;
        }
    }
}
