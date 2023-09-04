using ZZZ.Framework.Monogame.FarseerPhysics.Components;

namespace ZZZ.Framework.Monogame.FarseerPhysics.Extentions
{
    public static class SceneExtentions
    {
        public static Scene UseFarseerPhysics(this Scene scene, bool debug = false)
        {
            scene.AddComponent(new WorldRegistrar() {  Debug = debug, DrawOrder = 1});

            return scene;
        }
    }
}
