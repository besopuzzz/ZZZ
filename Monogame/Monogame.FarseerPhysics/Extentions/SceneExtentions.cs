using ZZZ.Framework.Monogame.FarseerPhysics.Components;

namespace ZZZ.Framework.Monogame.Auding.Extentions
{
    public static class SceneExtentions
    {
        public static Scene UseFarseerPhysics(this Scene scene, bool debug = false)
        {
            scene.AddComponent(new WorldRegistrar() {  Debug = debug});

            return scene;
        }
    }
}
